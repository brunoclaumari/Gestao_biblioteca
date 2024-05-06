
using AutoMapper;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoBiblioteca.Mappers
{
    public class BibliotecaMapper
    {
        private MapperConfiguration? _mapConfig;

        public BibliotecaMapper()
        {
            IniciaConfiguracao();
        }

        public MapperConfiguration? RetornaMapperConfiguration()
        {
            return _mapConfig;
        }

        private void IniciaConfiguracao()
        {
            if(_mapConfig == null)
            {                
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Livro, LivroDTO>().ReverseMap();
                });
                _mapConfig = config;
            }
        }


    }
}
