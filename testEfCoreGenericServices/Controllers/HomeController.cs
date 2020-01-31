using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GenericServices;
using GenericServices.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace testEfCoreGenericServices.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICrudServices _services;

        public HomeController(ICrudServices services)
        {
            _services = services;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return Ok(new { });
        }

        [Route("/read")]
        public async Task<IActionResult> Read()
        {
            var user = _services.ReadSingle<User>(a => a.Id == Guid.Parse("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"));

            Console.WriteLine("--- user ---");
            Console.WriteLine(user);

            var users = await _services.ReadManyNoTracked<User>().ToListAsync();

            Console.WriteLine("--- users ---");
            Console.WriteLine(JsonSerializer.Serialize(users));

            var usersDto = await _services.ProjectFromEntityToDto<User, UserDto>(a => a).ToListAsync();

            Console.WriteLine($"--- {nameof(usersDto)} ---");
            Console.WriteLine(JsonSerializer.Serialize(usersDto));

            var userDto = await _services.ProjectFromEntityToDto<User, UserDto>(a => a.Where(b => b.Id == Guid.Parse("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"))).FirstOrDefaultAsync();

            Console.WriteLine($"--- {nameof(userDto)} ---");
            Console.WriteLine(JsonSerializer.Serialize(userDto));

            return Ok();
        }

        [Route("/create")]
        public ActionResult<WebApiMessageAndResult<User>> Create()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "abc"
            };

            _services.CreateAndSave(user);

            Console.WriteLine("--- create ---");
            Console.WriteLine($"isvalid - {_services.IsValid}");
            Console.WriteLine($"message - {_services.Message}");
            Console.WriteLine($"errors - {JsonSerializer.Serialize(_services.Errors)}");

            return _services.Response(user);
        }

        [Route("/update")]
        public ActionResult<WebApiMessageOnly> Update()
        {
            var user = _services.ReadSingle<User>(a => a.Id == Guid.Parse("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"));
            user.Name = "abc123";

            _services.UpdateAndSave(user);

            Console.WriteLine("--- update ---");
            Console.WriteLine($"isvalid - {_services.IsValid}");
            Console.WriteLine($"message - {_services.Message}");
            Console.WriteLine($"errors - {JsonSerializer.Serialize(_services.Errors)}");

            return _services.Response();
        }
        
        [Route("/delete")]
        public ActionResult<WebApiMessageOnly> Delete()
        {
            _services.DeleteAndSave<User>(Guid.Parse("32feb0dd-6182-4dc0-9ad6-b147b6fb0cbb"));

            Console.WriteLine("--- update ---");
            Console.WriteLine($"isvalid - {_services.IsValid}");
            Console.WriteLine($"message - {_services.Message}");
            Console.WriteLine($"errors - {JsonSerializer.Serialize(_services.Errors)}");

            return _services.Response();
        }
        
        [Route("/delete2")]
        public ActionResult<WebApiMessageOnly> Delete2()
        {
            _services.DeleteWithActionAndSave<User>((context, user) =>
            {
                var status = new StatusGenericHandler();
                // status.AddError("error");
                
                status.Message = "success";

                var any = context.Set<User>().Any(a => a.Id == user.Id);

                Console.WriteLine($"--- {nameof(Delete2)} ---");
                Console.WriteLine($"any - {any}");

                return status;
            }, Guid.Parse("d88fbbb3-0858-43e1-b891-d49b32bfdece"));

            Console.WriteLine("--- update ---");
            Console.WriteLine($"isvalid - {_services.IsValid}");
            Console.WriteLine($"message - {_services.Message}");
            Console.WriteLine($"errors - {JsonSerializer.Serialize(_services.Errors)}");

            return _services.Response();
        }
    }

    public class UserDto : ILinkToEntity<User>
    {
        [ReadOnly(true)]
        public string Name { get; set; }

        public string Name2 { get; set; }
    }
}