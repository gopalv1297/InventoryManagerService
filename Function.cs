using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace InventoryManagerService
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");
            Feedback fb = new Feedback();

            if (request?.QueryStringParameters != null)
            {
                fb.Name = (request.QueryStringParameters.ContainsKey("name"))? request.QueryStringParameters["name"].ToString() : "force blank";
                fb.Comments = (request.QueryStringParameters.ContainsKey("comments"))? request.QueryStringParameters["comments"].ToString(): "force blank";
                fb.Email = (request.QueryStringParameters.ContainsKey("email")) ? request.QueryStringParameters["email"].ToString(): "force blank";
                fb.Experience = (request.QueryStringParameters.ContainsKey("experience")) ? request.QueryStringParameters["experience"].ToString():  "force blank";
            
                var requestContext = request.RequestContext;
                var sourceIP = requestContext?.Identity?.SourceIp
                var body = request.Body;

                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    //Body = "Gopals first lamda is alive",
                    Body = $"{fb.Name}'s (Email: {fb.Email}) experience was {fb.Experience}" +
                            $"with the following comments: {fb.Comments} sourceip {sourceIP} body:{body}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
                return response;
            }
            else
            {
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    //Body = "Gopals first lamda is alive",
                    Body = "No feedback provided from {0}",sourceIP,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
                return response;

            }

            //return new APIGatewayProxyResponse
            //{
            //    Body = "Body: " + request.Body
            //      + Environment.NewLine
            //      + "Querystring: " + (request.QueryStringParameters == null ? "null" : string.Join(",", request.QueryStringParameters.Keys))
            //      + Environment.NewLine +request.QueryStringParameters["name"].ToString() 
            //      +Environment.NewLine + request.QueryStringParameters["comments"].ToString(),
            //      //+ Environment.NewLine,
            //    StatusCode = 200
            //};


        }

        //public APIGatewayProxyResponse Get(Feedback fb, ILambdaContext context)
        //{
        //    context.Logger.LogLine("Get Request\n");

        //    var response = new APIGatewayProxyResponse
        //    {
        //        StatusCode = (int)HttpStatusCode.OK,

        //        Body = $"{fb.Name}'s (Email: {fb.Email}) experience was {fb.Experience}" +
        //        $"with the following comments: {fb.Comments} ",
        //        Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        //    };

        //    return response;
        //}



    }
}
