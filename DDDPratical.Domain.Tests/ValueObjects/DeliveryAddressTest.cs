using DDDPratical.Domain.Commom.Exceptions;
using DDDPratical.Domain.ValueObjects;
using FluentAssertions;

namespace DDDPratical.Domain.Tests.ValueObjects;

//Pattern to name tests: MethodName_ShouldExpectedBehavior_WhenStateUnderTest
public class DeliveryAddressTest
{

    [Fact(DisplayName = "A DeliveryAddress shoul be created when all data is valid")]
    public void Create_ShouldReturnValidAddress_WhenDataIsValid()
    {
        //Arrange
        var postalCode = "12345-678";
        var street = "Main St";
        var number = "100";
        var complement = "Apt 1";
        var neighborhood = "Downtown";
        var state = "CA";
        var city = "Los Angeles";
        var country = "USA";

        //Act
        var address = DeliveryAddress.Create(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );

        //Assert
        address.Should().NotBeNull();
        address.PostalCode.Should().Be(postalCode);
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.Complement.Should().Be(complement);
        address.Neighborhood.Should().Be(neighborhood);
        address.State.Should().Be(state);
        address.City.Should().Be(city);
        address.Country.Should().Be(country);
    }

    [Fact(DisplayName = "Two DeliveryAddress instances with same data should be equal")]
    public void Equals_ShouldReturnTrue_WhenAddressesHaveSameData()
    {
        //Arrange
        var postalCode = "12345-678";
        var street = "Main St";
        var number = "100";
        var complement = "Apt 1";
        var neighborhood = "Downtown";
        var state = "CA";
        var city = "Los Angeles";
        var country = "USA";

        var address1 = DeliveryAddress.Create(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );

        var address2 = DeliveryAddress.Create(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );

        //Act & Assert
        address1.Should().Be(address2);
        (address1 == address2).Should().BeTrue();
    }

    [Fact(DisplayName = "Two DeliveryAddress instances with different data should not be equal")]
    public void Equals_ShouldReturnFalse_WhenAddressesHaveDifferentData()
    {
        //Arrange
        var address1 = DeliveryAddress.Create(
            "12345-678", "Main St", "100", "Apt 1",
            "Downtown", "CA", "Los Angeles", "USA"
            );

        var address2 = DeliveryAddress.Create(
            "87654-321", "Second St", "200", "Apt 2",
            "Uptown", "NY", "New York", "USA"
            );


        //Act & Assert
        address1.Should().NotBe(address2);
        (address1 != address2).Should().BeTrue();
    }

    [Fact(DisplayName = "DeliveryAddress should be imutable")]
    public void Immutability_ShouldNotAllowModification_AfterCreation()
    {
        //Arrange
        var postalCode = "12345-678";
        var street = "Main St";
        var number = "100";
        var complement = "Apt 1";
        var neighborhood = "Downtown";
        var state = "CA";
        var city = "Los Angeles";
        var country = "USA";
        var address = DeliveryAddress.Create(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );

        //Act & Assert
        // Attempting to modify properties should result in a compile-time error.
        // The following lines are commented out to indicate that they should not compile.
        // address.Street = "New St"; // This line should cause a compile-time error
        // address.Number = "200";     // This line should cause a compile-time error
        
        address.GetType().GetProperties()
            .All(prop => prop.SetMethod == null || prop.SetMethod.IsPrivate)
            .Should().BeTrue();
    }

    [Theory(DisplayName = "A DeliveryAddress should throw DomainException when required fields are null or whitespace")]
    [InlineData(null, "Main St", "100", "Apt 1", "Downtown", "CA", "Los Angeles", "USA", "postalCode")]
    [InlineData("12345-678", "", "100", "Apt 1", "Downtown", "CA", "Los Angeles", "USA", "street")]
    [InlineData("12345-678", "Main St", "100", "Apt 1", " ", "CA", "Los Angeles", "USA", "neighborhood")]
    [InlineData("12345-678", "Main St", "100", "Apt 1", "Downtown", "CA", "Los Angeles", null, "country")]
    public void Create_ShouldThrowDomainException_WhenRequiredFieldsAreNullOrWhiteSpace(
        string postalCode, string street, string number, string complement,
        string neighborhood, string state, string city, string country, string expectedParamName)
    {
        //Act
        Action act = () => DeliveryAddress.Create(
            postalCode, street, number, complement,
            neighborhood, state, city, country
            );

        //Assert
        act.Should().Throw<DomainException>()
            .Where(e => e.Message.Contains(expectedParamName));
    }

    [Theory(DisplayName = "A DeliveryAddress should throw DomainException when postal code is invalid")]
    [InlineData("12345678")]
    [InlineData("1234-5678")]
    [InlineData("ABCDE-678")]
    public void Create_ShouldThrowDomainException_WhenPostalCodeIsInvalid(string invalidPostalCode)
    {
        //Arrange
        var street = "Main St";
        var number = "100";
        var complement = "Apt 1";
        var neighborhood = "Downtown";
        var state = "CA";
        var city = "Los Angeles";
        var country = "USA";

        //Act
        Action act = () => DeliveryAddress.Create(
            invalidPostalCode, street, number, complement,
            neighborhood, state, city, country
            );

        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Invalid postal code format*");
    }

}
