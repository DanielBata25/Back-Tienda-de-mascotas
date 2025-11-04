using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Data.Init
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ejecuta migraciones pendientes
            context.Database.Migrate();


            if (!context.Clientes.Any())
            {
                var cliente1 = new Cliente
                {
                    Nombre = "Camilo",
                    Apellido = "Charry",
                    Email = "camilo.charry@correo.com",
                    Telefono = "3001234567",
                    Direccion = "Neiva - Huila",
                    IsDeleted = false
                };

                var cliente2 = new Cliente
                {
                    Nombre = "Laura",
                    Apellido = "Michel",
                    Email = "laura.michel@correo.com",
                    Telefono = "3109876543",
                    Direccion = "Pitalito - Huila",
                    IsDeleted = false
                };

                context.Clientes.AddRange(cliente1, cliente2);
                context.SaveChanges();
            }


            if (!context.Mascotas.Any())
            {
                var mascota1 = new Mascota
                {
                    Nombre = "Luna",
                    Especie = "Perro",
                    Raza = "Labrador",
                    FechaNacimiento = new DateTime(2020, 3, 15),
                    Genero = "Hembra",
                    Notas = "Amigable y activa",
                    IsDeleted = false
                };

                var mascota2 = new Mascota
                {
                    Nombre = "Simba",
                    Especie = "Gato",
                    Raza = "Angora",
                    FechaNacimiento = new DateTime(2021, 11, 2),
                    Genero = "Macho",
                    Notas = "Curioso y dormilón",
                    IsDeleted = false
                };

                context.Mascotas.AddRange(mascota1, mascota2);
                context.SaveChanges();
            }


            if (!context.MascotaClientes.Any())
            {
                var mascotaCliente1 = new MascotaCliente
                {
                    MascotaId = context.Mascotas.First().Id,
                    ClienteId = context.Clientes.First().Id,
                    FechaRegistro = DateTime.Now,
                    IsDeleted = false
                };

                var mascotaCliente2 = new MascotaCliente
                {
                    MascotaId = context.Mascotas.Last().Id,
                    ClienteId = context.Clientes.Last().Id,
                    FechaRegistro = DateTime.Now,
                    IsDeleted = false
                };

                context.MascotaClientes.AddRange(mascotaCliente1, mascotaCliente2);
                context.SaveChanges();
            }

            if (!context.Productos.Any())
            {
                var producto1 = new Producto
                {
                    Nombre = "Concentrado DogChow",
                    Categoria = "Alimento",
                    Descripcion = "Bolsa de 10kg para perro adulto",
                    Stock = 25,
                    Estado = "Disponible",
                    IsDeleted = false
                };

                var producto2 = new Producto
                {
                    Nombre = "Arena para gatos GatoClean",
                    Categoria = "Higiene",
                    Descripcion = "Bolsa de 5kg, neutraliza olores",
                    Stock = 50,
                    Estado = "Disponible",
                    IsDeleted = false
                };

                context.Productos.AddRange(producto1, producto2);
                context.SaveChanges();
            }

            if (!context.Ventas.Any())
            {
                var venta1 = new Venta
                {
                    ClienteId = context.Clientes.First().Id,
                    FechaVenta = DateTime.Now,
                    Estado = "Completada",
                    Canal = "Tienda Física",
                    Observaciones = "Pago en efectivo",
                    IsDeleted = false
                };

                context.Ventas.Add(venta1);
                context.SaveChanges();
            }

            if (!context.VentaProductos.Any())
            {
                var venta = context.Ventas.First();
                var producto = context.Productos.First();

                var ventaProducto = new VentaProducto
                {
                    VentaId = venta.Id,
                    ProductoId = producto.Id,
                    Cantidad = 2,
                    PrecioUnitario = 85000m,
                    IsDeleted = false
                };

                context.VentaProductos.Add(ventaProducto);
                context.SaveChanges();
            }


            if (!context.Pagos.Any())
            {
                var pago1 = new Pago
                {
                    VentaId = context.Ventas.First().Id,
                    FechaPago = DateTime.Now,
                    Monto = 170000m,
                    MetodoPago = "Efectivo",
                    EstadoPago = "Aprobado",
                    Referencia = "PAG-" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    IsDeleted = false
                };

                context.Pagos.Add(pago1);
                context.SaveChanges();
            }
        }
    }
}
