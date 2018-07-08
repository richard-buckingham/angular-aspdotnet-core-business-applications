using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using TourManagement.API.Helpers;
using Xunit;

namespace TourManagement.UnitTests
{
    public class RequestHeaderMatchesMediaTypeAttributeTests
    {
        [Fact]
        public void WhenHeaderTypeAndMediaTypesMatch_ExpectAccepted()
        {
            // Arrange
            var attribute = new RequestHeaderMatchesMediaTypeAttribute("Accept", new[] { "application/vnd.marvin.tour+json" } );

            var requestHeaders = new HeaderDictionary();
            requestHeaders.Add("Accept", new[] { "application/vnd.marvin.tour+json" });

            // Act
            bool acceptRequest = attribute.AcceptRequestHeaders(requestHeaders);

            //Assert
            Assert.True(acceptRequest);
        }

        [Fact]
        public void WhenHeaderTypeMatchesButMediaTypesNotMatched_ExpectNotAccepted()
        {
            // Arrange
            var attribute = new RequestHeaderMatchesMediaTypeAttribute("Accept", new[] { "application/vnd.marvin.tourwhatever+json" });

            var requestHeaders = new HeaderDictionary();
            requestHeaders.Add("Accept", new[] { "application/vnd.marvin.tour+json" });

            // Act
            bool acceptRequest = attribute.AcceptRequestHeaders(requestHeaders);

            //Assert
            Assert.False(acceptRequest);
        }

        [Fact]
        public void WhenHeaderTypeNotMatchesButMediaTypesMatches_ExpectNotAccepted()
        {
            // Arrange
            var attribute = new RequestHeaderMatchesMediaTypeAttribute("Accept", new[] { "application/vnd.marvin.tour+json" });

            var requestHeaders = new HeaderDictionary();
            requestHeaders.Add("Content-Type", new[] { "application/vnd.marvin.tour+json" });

            // Act
            bool acceptRequest = attribute.AcceptRequestHeaders(requestHeaders);

            //Assert
            Assert.False(acceptRequest);
        }
    }
}


