using WebApplication1.Attributes;

namespace WebApplication1.Repositories
{
    [Singleton]
    public class ExemploRepository : IExemploRepository
    {
        private int contadorRepo = 0;
        public string Testar()
        {
            contadorRepo++;
            return $"teste repo: {contadorRepo}";
        }
    }
}
