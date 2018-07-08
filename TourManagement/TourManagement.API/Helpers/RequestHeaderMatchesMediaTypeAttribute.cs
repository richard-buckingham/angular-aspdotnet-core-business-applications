using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.API.Helpers
{
    // allow multiple instances of this attribute to be attached to the same action
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly string _requestHeaderToMatch;

        private readonly string[] _mediaTypes;

        public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch, string[] mediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch;
            _mediaTypes = mediaTypes;
        }

        public int Order {
            get { return 0; }
        }

        public bool AcceptRequestHeaders(IHeaderDictionary contextRequestHeaders)
        {
            // check request headers
            if (!contextRequestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }

            // if one of the media types matches, return true
            foreach (var mediaType in _mediaTypes.ToList())
            {
                var matchingRequestHeader = contextRequestHeaders[_requestHeaderToMatch];
                var headerValues = matchingRequestHeader.ToString().Split(',').ToList();

                foreach (var headerValue in headerValues)
                {
                    if (String.Equals(headerValue, mediaType, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;

            return AcceptRequestHeaders(requestHeaders);
        }
    }
}
