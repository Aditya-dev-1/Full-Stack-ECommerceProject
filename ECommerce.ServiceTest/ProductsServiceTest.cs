using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using ECommerce.Core.Domain.Entities;
using ECommerce.Core.DTO;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace ECommerce.ServiceTest
{
    public class ProductsServiceTest
    {
        private readonly IProductsAdderService _productsAdderService;
        private readonly IProductsGetterService _productsGetterService;
        private readonly IProductsDeleteService _productsDeleteService;
        private readonly IProductsUpdateService _productsUpdateService;


        private readonly IProductsGetterService _mockProductsGetterServiceObject;

        private readonly IProductsRepository _productsRepository;
        private readonly IImageAdderService _imageAdderService;
        private readonly IImageDeleterService _imageDeleterService;
        private readonly IImageUpdaterService _imageUpdaterService;
        private readonly ICategoriesGetterService _categoriesGetterService;

        private readonly Mock<IProductsRepository> _mockProductsRepository;
        private readonly Mock<IImageAdderService> _mockImageAdderService;
        private readonly Mock<IImageDeleterService> _mockImageDeleterService;
        private readonly Mock<IImageUpdaterService> _mockImageUpdaterService;
        private readonly Mock<ICategoriesGetterService> _mockCategoriesGetterService;
        private readonly Mock<IProductsGetterService> _mockProductsGetterService;

        private readonly IFixture _fixture;

        public ProductsServiceTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization()); // customizing autofixture as it can't directly create the dummy values of those fileds which is of Interface type (ProductImage IFormFile) 

            _mockProductsRepository = new Mock<IProductsRepository>();
            _mockImageAdderService = new Mock<IImageAdderService>();
            _mockImageDeleterService = new Mock<IImageDeleterService>();
            _mockImageUpdaterService = new Mock<IImageUpdaterService>();
            _mockCategoriesGetterService = new Mock<ICategoriesGetterService>();
            _mockProductsGetterService = new Mock<IProductsGetterService>();


            _productsRepository = _mockProductsRepository.Object;
            _imageAdderService = _mockImageAdderService.Object;
            _imageDeleterService = _mockImageDeleterService.Object;
            _imageUpdaterService = _mockImageUpdaterService.Object;
            _categoriesGetterService = _mockCategoriesGetterService.Object;
            _mockProductsGetterServiceObject = _mockProductsGetterService.Object;

            _productsAdderService = new ProductsAdderService(_productsRepository,_imageAdderService);

            _productsGetterService = new ProductsGetterService(_productsRepository, _categoriesGetterService);

            _productsDeleteService = new ProductsDeleteService(_productsRepository,_imageDeleterService);

            _productsUpdateService = new ProductsUpdateService(_productsRepository,_imageUpdaterService);
        }


        #region AddProduct

        //when the prdocustAddRequest is null it should throw ArgumentNullException
        [Fact]
        public async Task AddPrdouct_NullProduct_ToBeNullArgumentException()
        {
            //Arrange
            ProductsAddRequest? productAddRequest = null;

            
            Func<Task> action = async () =>
            {
                //Act
                await _productsAdderService.AddProduct(productAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //when productImage is null it should throw ArgumentException
        [Fact]
        public async Task AddPrdouct_NullProductImage_ToBeArgumentException()
        {
            //Arrange
            ProductsAddRequest? productAddRequest = _fixture.Build<ProductsAddRequest>()
                .With(temp => temp.Product_Image, null as IFormFile)
                .Create();


            Func<Task> action = async () =>
            {
                //Act
                await _productsAdderService.AddProduct(productAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when proper product details is passed  it should be successfull
        [Fact]
        public async Task AddPrdouct_FullProductDetails_ToBeSuccessfull()
        {
            //creating mock obejct for IFormFile (PrdocutImage)
            var mockFormFile = _fixture.Create<Mock<IFormFile>>();
            mockFormFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFormFile.Setup(f => f.Length).Returns(1024);

            //Arrange
            ProductsAddRequest productAddRequest = _fixture.Build<ProductsAddRequest>()
                .With(temp=>temp.Product_Image,mockFormFile.Object)
                .Create();

            Product product = productAddRequest.ToProduct();
            ProductResponse productResponseExpected = product.ToProductResponse();

            //Mocking productsRepository addProduct Method
            _mockProductsRepository.Setup(temp => temp.AddProduct(It.IsAny<Product>()))
                .ReturnsAsync(product);

            _mockImageAdderService.Setup(temp => temp.ImageAdder(It.IsAny<IFormFile>(),It.IsAny<string>()))
                .ReturnsAsync("dummy_path.jpg");

            //Act
           ProductResponse productResponseActual =  await _productsAdderService.AddProduct(productAddRequest);

            productResponseExpected.ProductId = productResponseActual.ProductId;

            productResponseActual.Product_Image_Path = productResponseExpected.Product_Image_Path;

            //Assert
            productResponseActual.Should().NotBeNull();
            productResponseActual.Should().BeEquivalentTo(productResponseExpected);
        }

        #endregion

        #region DeleteProduct

        [Fact]
        public async Task DeleteProduct_InvalidProductId_ShouldBeFalse()
        {
            //Act
            bool isDeleted = await _productsDeleteService.DeleteProduct(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteProduct_ValidProductId_ShouldBeTrue()
        {
            //Arrange
            Product product = _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .Create();

            _mockProductsRepository.Setup(temp => temp.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(product);

            _mockProductsRepository.Setup(temp => temp.DeleteProductByProductId(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _mockImageDeleterService.Setup(temp => temp.ImageDeleter(It.IsAny<string>()))
                .ReturnsAsync(true);

            //Act
            bool isDeleted = await _productsDeleteService.DeleteProduct(product.Id);

            //Assert
            isDeleted.Should().BeTrue();
        }

        #endregion

        #region GetAllProducts

        //When products in db is empty it should return empty
        [Fact]
        public async Task GetAllProducts_ByDefault_ShouldBeEmpty()
        {
            //Arrange
            List<Product> products = new List<Product>();

            _mockProductsRepository.Setup(temp=>temp.GetAllProducts())
                .ReturnsAsync(products);

            //Act
           List<ProductResponse> productResponseActual = await  _productsGetterService.GetAllProducts();

           //Assert
           productResponseActual.Should().BeEmpty();

        }

        //When one or more products are in db it should return them.
        [Fact]
        public async Task GetAllProducts_ProperList_ShouldBeSuccessfull()
        {
            //Arrange
            List<Product> products = new List<Product>()
            {
                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .Create(),

                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .Create(),

                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .Create()
            };

            _mockProductsRepository.Setup(temp => temp.GetAllProducts())
                .ReturnsAsync(products);

            List<ProductResponse> productResponseExpected_list = products.Select(temp=>temp.ToProductResponse()).ToList();

            //Act
            List<ProductResponse> productResponseActual_list = await _productsGetterService.GetAllProducts();

            //Assert
            productResponseActual_list.Should().BeEquivalentTo(productResponseExpected_list);

        }

        #endregion

        #region GetProductById

        //when invalid product is send it should return null
        [Fact]
        public async Task GetProductById_InvalidId_ShouldReturnNull()
        {
            //Arrange
            Guid? productId = null;

            //Act
            ProductResponse? productResponseActual = await _productsGetterService.GetProductByProductId(productId);

            //Assert
            productResponseActual.Should().BeNull();
        }

        //when valid product is send it should return Mathcing Product
        [Fact]
        public async Task GetProductById_ValidId_ShouldBeSuccessfull()
        {
            //Arrange
            Product product = _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .Create();

            _mockProductsRepository.Setup(temp=>temp.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(product);

            ProductResponse productResponseExpected = product.ToProductResponse(); 

            //Act
            ProductResponse? productResponseActual = await _productsGetterService.GetProductByProductId(product.Id);

            //Assert
            productResponseActual.Should().BeEquivalentTo(productResponseExpected);
        }

        #endregion

        #region GetProductsBasedOnCategoryId

        //when categoryId is invalid it should return null
        [Fact]
        public async Task GetProductsBasedOnCategoryId_InvalidCategoryId_ShouldBeNull()
        {
            //Arrange
            
            CategoryResponse? categoryResponse = null;

            _mockCategoriesGetterService.Setup(temp => temp.GetCategoryByCategoryId(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            //Act
            List<ProductResponse>? productResponseActual = await _productsGetterService.GetProductsBasedOnCategoryId(Guid.NewGuid());

            //Assert
            productResponseActual.Should().BeNull();
        }

        //when categoryId is valid but that category does not have any products so it should return null
        [Fact]
        public async Task GetProductsBasedOnCategoryId_ValidIdWithNoProducts_ShouldBeNull()
        {
            //Arrange
            Category category = _fixture.Build<Category>()
                .With(temp=>temp.Products,null as ICollection<Product>)
                .Create();

            CategoryResponse categoryResponse = category.ToCategoryResponse();

            List<Product>? products = null;

            _mockCategoriesGetterService.Setup(temp => temp.GetCategoryByCategoryId(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            _mockProductsRepository.Setup(temp=>temp.GetProductsBasedOnCategoryId(It.IsAny<Guid>()))
                .ReturnsAsync(products);

            //Act
            List<ProductResponse>? productResponseActual = await _productsGetterService.GetProductsBasedOnCategoryId(category.Id);

            //Assert
            productResponseActual.Should().BeNull();
        }

        //when categoryId is valid and that category has one or more products so it should return them
        [Fact]
        public async Task GetProductsBasedOnCategoryId_ValidIdWithProducts_ShouldBeSuccessfull()
        {
            //Arrange
            Category category = _fixture.Build<Category>()
                .With(temp => temp.Products, null as ICollection<Product>)
                .Create();

            CategoryResponse categoryResponse = category.ToCategoryResponse();

            List<Product> products = new List<Product>()
            {
                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .With(temp=>temp.CategoryId,category.Id)
                .Create(),

                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .With(temp=>temp.CategoryId,category.Id)
                .Create(),

                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .With(temp=>temp.CategoryId,category.Id)
                .Create()
            }; 

            List<ProductResponse> productResponsesExpected_list = products.Select(temp=>temp.ToProductResponse())
                .ToList();

            _mockCategoriesGetterService.Setup(temp => temp.GetCategoryByCategoryId(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            _mockProductsRepository.Setup(temp => temp.GetProductsBasedOnCategoryId(It.IsAny<Guid>()))
                .ReturnsAsync(products);

            //Act
            List<ProductResponse>? productResponseActual_list = await _productsGetterService.GetProductsBasedOnCategoryId(category.Id);

            //Assert
            productResponseActual_list.Should().NotBeNull();
            productResponseActual_list.Should().BeEquivalentTo(productResponsesExpected_list);
        }

        #endregion

        #region GetFilteredProducts

        //when searchString is  null it should return all Products
        [Fact]
        public async Task GetFilteredProducts_SearchStringIsNull_ShouldReturnAllProducts()
        {
            //Arrange
            string? searchString = null;

            List<ProductResponse> productResponseExpected_list = new List<ProductResponse>()
            {
                _fixture.Build<ProductResponse>()
                .Create(),

                _fixture.Build<ProductResponse>()
                .Create()
            };

            _mockProductsGetterService.Setup(temp=>temp.GetAllProducts())
                .ReturnsAsync(productResponseExpected_list);

            //Act
            List<ProductResponse> productsResponseActual_List = await _productsGetterService.GetFilteredProducts(searchString);

            //Assert
            productsResponseActual_List.Should().NotBeNullOrEmpty();
            productsResponseActual_List.Should().BeEquivalentTo(productResponseExpected_list);
        }

        //when searchString is not null it should return matchingProducts
        [Fact]
        public async Task GetFilteredProducts_SearchStringIsNotNull_ShouldReturnMatchingProducts()
        {
            //Arrange
            string searchString = "sa";

            List<Product> products = new List<Product>()
            {
                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .With(temp=>temp.Product_Name,"samsung a70")
                .Create(),

                _fixture.Build<Product>()
                .With(temp=>temp.CartItems,null as ICollection<Carts>)
                .With(temp=>temp.Categories,null as Category)
                .With(temp=>temp.Product_Name,"samsung s23")
                .Create()
            };

            List<ProductResponse> productResponsesExpected_list = products.Select(temp => temp.ToProductResponse())
               .ToList();

            _mockProductsRepository.Setup(temp => temp.GetFilteredProducts(It.IsAny<string>()))
                .ReturnsAsync(products);


            //Act
            List<ProductResponse> productsResponseActual_List = await _productsGetterService.GetFilteredProducts(searchString);

            //Assert
            productsResponseActual_List.Should().NotBeNullOrEmpty();
            productsResponseActual_List.Should().BeEquivalentTo(productResponsesExpected_list);
        }

        #endregion

        #region UpdateProduct
        #endregion
    }
}
