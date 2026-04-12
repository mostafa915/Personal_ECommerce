using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Application.Abstractions
{
    public static class ResultExteions
    {
        public static ObjectResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Can not convert result to problem as it is succuess");

            var problem = Results.Problem(statusCode: result.Error.StatusCode);

            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {"errors", new []
                {
                    result.Error.Code,
                    result.Error.Description
                } }
            };


            return new ObjectResult(problemDetails);
        }
    }
}
