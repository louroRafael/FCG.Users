using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FCG.Users.API.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (!type.IsEnum) return;

            schema.Type = "string";
            schema.Enum.Clear();

            foreach (var name in Enum.GetNames(type))
            {
                schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name));
            }
        }
    }
}
