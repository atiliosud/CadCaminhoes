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
    public class CaminhaoModelTest
    {


        [Fact]
        public async Task Validate_OK()
        {

            var user = FactoryUser();
            var caminhao = FactoryCaminhao(user);
            // Set some properties here
            var context = new ValidationContext(caminhao, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Caminhao), typeof(Caminhao)), typeof(Caminhao));

            var isModelStateValid = Validator.TryValidateObject(caminhao, context, results, true);

            // Assert here
            isModelStateValid.Should().BeTrue();

        }


        [Fact]
        public async Task Validate_Modelo_NaoPermitido()
        {

            var user = FactoryUser();
            var caminhao = FactoryCaminhao(user);
            caminhao.Modelo = "QWEQWEWQE";
            // Set some properties here
            var context = new ValidationContext(caminhao, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Caminhao), typeof(Caminhao)), typeof(Caminhao));

            var isModelStateValid = Validator.TryValidateObject(caminhao, context, results, true);

            // Assert here
            isModelStateValid.Should().BeFalse();

            results.Should().Contain(x => x.ErrorMessage == "modelo não permitido.");
        }


        [Fact]
        public async Task Validate_FabricacaoAno_Atual()
        {

            var user = FactoryUser();
            var caminhao = FactoryCaminhao(user);
            caminhao.FabricacaoAno = 2010;
            // Set some properties here
            var context = new ValidationContext(caminhao, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Caminhao), typeof(Caminhao)), typeof(Caminhao));

            var isModelStateValid = Validator.TryValidateObject(caminhao, context, results, true);

            // Assert here
            isModelStateValid.Should().BeFalse();

            results.Should().Contain(x => x.ErrorMessage == $"somente o ano {DateTimeOffset.Now.Year} é permitido.");
        }


        [Fact]
        public async Task Validate_ModeloAno_Atual()
        {

            var user = FactoryUser();
            var caminhao = FactoryCaminhao(user);
            caminhao.ModeloAno = 2010;
            // Set some properties here
            var context = new ValidationContext(caminhao, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Caminhao), typeof(Caminhao)), typeof(Caminhao));

            var isModelStateValid = Validator.TryValidateObject(caminhao, context, results, true);

            // Assert here
            isModelStateValid.Should().BeFalse();

            results.Should().Contain(x => x.ErrorMessage == $"somente o ano {DateTimeOffset.Now.Year} e {DateTimeOffset.Now.Year + 1} são permitidos.");
        }



        internal static Caminhao FactoryCaminhao(IdentityUser user)
        {
            return new Caminhao()
            {
                Modelo = "FH",
                ModeloAno = 2023,
                FabricacaoAno = 2022,
                CreateById = user.Id,
                CreateAt = DateTimeOffset.UtcNow.AddDays(-10),
            };
        }


        internal static IdentityUser FactoryUser()
        {
            return new IdentityUser()
            {
                Id = "tester",
                Email = "teste@teste.com"
            };
        }
    }
}