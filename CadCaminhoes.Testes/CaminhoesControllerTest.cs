using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CadCaminhoes.MVC.Controllers;
using CadCaminhoes.MVC.Data;
using CadCaminhoes.MVC.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using Xunit;

namespace CadCaminhoes.Testes
{
    public class CaminhoesControllerTest
    {

        private readonly CaminhoesController _controller;
        private readonly ApplicationDbContext _context;

        public CaminhoesControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "cadcaminhoes")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new CaminhoesController(_context);
        }

        [Fact]
        public async Task Get_Index_Flow()
        {
            var user = FactoryUser();
            _context.Add(user);
            _context.Add(FactoryCaminhao(user));
            _context.Add(FactoryCaminhao(user));
            _context.Add(FactoryCaminhao(user));
            _context.Add(FactoryCaminhao(user));
            await _context.SaveChangesAsync();

            var result = await _controller.Index();

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult!.Model.Should().NotBeNull();
            viewResult!.Model.Should().BeOfType<List<Caminhao>>();

            var caminhoes = viewResult.Model as List<Caminhao>;
            caminhoes.Should().NotBeNull();
            caminhoes!.Count.Should().BeLessThanOrEqualTo(4);

        }

        [Fact]
        public async Task Get_Details_NotFound()
        {
            var result = await _controller.Details(null);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_Details_NotFound2()
        {
            var result = await _controller.Details(Guid.NewGuid());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_Details_Equals()
        {
            var user = FactoryUser();
            _context.Add(user);
            var caminhao = FactoryCaminhao(user);
            _context.Add(caminhao);
            _context.Add(FactoryCaminhao(user));
            _context.Add(FactoryCaminhao(user));
            _context.Add(FactoryCaminhao(user));
            await _context.SaveChangesAsync();


            var result = await _controller.Details(caminhao.Id);

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult!.Model.Should().NotBeNull();
            viewResult!.Model.Should().BeOfType<Caminhao>();

            var caminhaoResult = viewResult.Model as Caminhao;
            caminhaoResult.Should().NotBeNull();
            caminhaoResult!.Modelo.Should().Be(caminhao.Modelo);
            caminhaoResult!.ModeloAno.Should().Be(caminhao.ModeloAno);
            caminhaoResult!.FabricacaoAno.Should().Be(caminhao.FabricacaoAno);
            caminhaoResult!.CreateById.Should().Be(caminhao.CreateById);
            caminhaoResult!.CreateAt.Should().Be(caminhao.CreateAt);
        }


        [Fact]
        public void Get_Create_Ok()
        {
            var result = _controller.Create();
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Post_Create_Equals()
        {
            var user = FactoryUser();
            ClaimUser(user);
            _context.Add(user);
            await _context.SaveChangesAsync();
            var caminhao = FactoryCaminhao(user);

            var result = await _controller.Create(caminhao);

            result.Should().BeOfType<RedirectToActionResult>();


            var caminhaoExpected = await _context.Caminhoes.FirstOrDefaultAsync(x => x.Id == caminhao.Id);
            caminhaoExpected.Should().NotBeNull();
            caminhaoExpected.Modelo.Should().Be(caminhao.Modelo);
            caminhaoExpected.FabricacaoAno.Should().Be(caminhao.FabricacaoAno);
            caminhaoExpected.ModeloAno.Should().Be(caminhao.ModeloAno);
        }



        internal static Caminhao FactoryCaminhao(IdentityUser user)
        {
            return new Caminhao()
            {
                Modelo = "FH",
                ModeloAno = 2022,
                FabricacaoAno = 2023,
                CreateById = user.Id,
                CreateAt = DateTimeOffset.UtcNow.AddDays(-10),
            };
        }


        internal static IdentityUser FactoryUser()
        {
            var id = Guid.NewGuid().ToString();
            return new IdentityUser()
            {
                Id = id,
                Email = id + "@teste.com"
            };
        }


        public void ClaimUser(IdentityUser user)
        {
            var userClaim = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                user.Id
                ) }, "tester"));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = userClaim };

        }
    }
}