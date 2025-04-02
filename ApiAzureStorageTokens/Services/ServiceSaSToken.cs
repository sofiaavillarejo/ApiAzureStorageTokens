using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiAzureStorageTokens.Services
{
    public class ServiceSaSToken
    {
        private TableClient tablaAlumnos;

        public ServiceSaSToken(IConfiguration configuration)
        {
            //COGEMOS LAS CLAVES DE AZURE
            string azureKeys = configuration.GetValue<string>("AzureKeys:StorageAccount");
            TableServiceClient serviceClient = new TableServiceClient(azureKeys);
            this.tablaAlumnos = serviceClient.GetTableClient("alumnos");
        }

        //genera tokens para la tabla alumnos -> para el curso que nos envien
        public string GenerateToken(string curso)
        {
            //NECESITAMOS LOS PERMISOS DE ACCESO, POR AHORA SOLO PERMITIREMOS LA LECTURA
            TableSasPermissions permisos = TableSasPermissions.Read | TableSasPermissions.Add;
            //EL ACCESO AL TOKEN ESTA DELIMITADO POR UN TIEMPO -> NECESITAMOS INCLUIR EL TIEMPO QUE TENDRA DE ACCESO A LEER LOS ELEMENTOS
            TableSasBuilder builder = this.tablaAlumnos.GetSasBuilder(permisos, DateTime.UtcNow.AddMinutes(30));
            //COMO ROW KEY Y PARTITION KEY SON STRING PODEMOS LIMITAR EL ACCESO DE FORMA ALFABETICA A LOS DATOS, YA SEA POR ROW KEY, PARTITION KEY O AMBAS
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;
            //CON ESTO YA PODEMOS GENERAR EL TOKEN QUE ES UN ACCESO POR URI
            Uri uriToken = this.tablaAlumnos.GenerateSasUri(builder);
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
