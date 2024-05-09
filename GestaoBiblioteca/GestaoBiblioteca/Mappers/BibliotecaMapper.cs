
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
            //IniciaConfiguracao();
        }

        public MapperConfiguration RetornaMapperConfiguration()
        {
            //return _mapConfig;
            return _mapConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<Livro, LivroDTOEntrada>().ReverseMap();
                cfg.CreateMap<Livro, LivroDTORetorno>().ReverseMap();

                cfg.CreateMap<Usuario, UsuarioDTO>().ReverseMap();
                cfg.CreateMap<Usuario, UsuarioDTORetorno>().ReverseMap();
            });
        }

        private void IniciaConfiguracao()
        {
            if(_mapConfig == null)
            {
                _mapConfig = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Livro, LivroDTOEntrada>().ReverseMap();
                    cfg.CreateMap<Livro, LivroDTORetorno>().ReverseMap();
                });
                //_mapConfig = config;
                
            }
        }


    }
}
