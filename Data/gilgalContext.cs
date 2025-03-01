using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GilgalInventar.Data
{
    public class gilgalContext : IdentityDbContext
    {
        public gilgalContext(DbContextOptions<gilgalContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Archivo> Archivos { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AreaFuncional> AreaFuncionals { get; set; }
        public virtual DbSet<Bodega> Bodegas { get; set; }
        public virtual DbSet<Cara> Caras { get; set; }
        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<CentroCosto> CentroCostos { get; set; }
        public virtual DbSet<ConceptoRemision> ConceptoRemisions { get; set; }
        public virtual DbSet<CondicionEquipo> CondicionEquipos { get; set; }
        public virtual DbSet<ElementoEquipo> ElementoEquipos { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Entrega> Entregas { get; set; }
        public virtual DbSet<EntregaDetalle> EntregaDetalles { get; set; }
        public virtual DbSet<EstadoEquElem> EstadoEquElems { get; set; }
        public virtual DbSet<Estante> Estantes { get; set; }
        public virtual DbSet<GrupoResponsable> GrupoResponsables { get; set; }
        public virtual DbSet<Marca> Marcas { get; set; }
        public virtual DbSet<Nivele> Niveles { get; set; }
        public virtual DbSet<OrigenDestino> OrigenDestinos { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Proveedore> Proveedores { get; set; }
        public virtual DbSet<MovimientoDetalle> MovimientoDetalles { get; set; }
        public virtual DbSet<Movimiento> Movimientos { get; set; }
        public virtual DbSet<TipoEquipoElemento> TipoEquipoElementos { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacions { get; set; }
        public virtual DbSet<Unidade> Unidades { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Zona> Zonas { get; set; }
        public virtual DbSet<TipoMovimiento> TipoMovimientos { get; set; }
        public virtual DbSet<Formato> Formatos { get; set; }
        public virtual DbSet<AreaElemento> AreaElementos { get; set; }
        public virtual DbSet<ArchivoEtiqueta> ArchivoEtiquetas { get; set; }
        public virtual DbSet<RemisionDetalleEntregaEPP> RemisionDetalleEntregaEPPs { get; set; }
        public virtual DbSet<sp_AspNetUsers> sp_AspNetUsers { get; set; }
        public virtual DbSet<sp_AspNetRoles> sp_AspNetRoles { get; set; }
        public virtual DbSet<sp_AspNetUserRoles> sp_AspNetUserRoles { get; set; }
        public virtual DbSet<sp_getEntregasEPP> sp_getEntregasEPP { get; set; }
        public virtual DbSet<sp_ActualizarExistencias> sp_ActualizarExistencias { get; set; }
        public virtual DbSet<sp_ActualizarExistenciasElemento> sp_ActualizarExistenciasElemento { get; set; }
        public virtual DbSet<sp_MovimientoInventario> sp_MovimientoInventario { get; set; }
        public virtual DbSet<Requisisione> Requisisiones { get; set; }
        public virtual DbSet<RequisisionDetalle> RequisisionDetalles { get; set; }
        public virtual DbSet<sp_ConsultaKardexInventario> sp_ConsultaKardexInventario { get; set; }
        public virtual DbSet<Movimiento> sp_ActualizarMovimientoES { get; set; }
    }
}
