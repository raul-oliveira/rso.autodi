using WebApplication1.Attributes;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    [Transient]
    public class ExemploService : IExemploService
    {
        private readonly IExemploRepository exemploRepository;
        private int contador = 0;

        public ExemploService(IExemploRepository exemploRepository)
        {
            this.exemploRepository = exemploRepository;
        }
        
        public string ObterTeste()
        {
            contador += 2;
            return exemploRepository.Testar() + $"service: {contador}";
        }
    }
}
