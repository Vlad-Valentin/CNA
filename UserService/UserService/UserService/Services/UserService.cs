using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserService
{
    public class UserService : UserManager.UserManagerBase
    {
        public override Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var user = request.User;

            if (user.Name.Equals("Invalid Name"))
            {
                Console.WriteLine("\nName Is Blank!");
                return Task.FromResult(new AddUserResponse() { Status = AddUserResponse.Types.Status.Error, Name = "Invalid Name" });
            }

            if (user.CNP.Equals("Invalid ID"))
            {
                Console.WriteLine("\nInvalid ID Length!");
                return Task.FromResult(new AddUserResponse() { Status = AddUserResponse.Types.Status.Error });
            }

            Console.WriteLine($"\nName: {user.Name}");

            DateTime birthDate = default;
            int birthYear = int.Parse(user.CNP.Substring(1, 2));
            int birthMonth = int.Parse(user.CNP.Substring(3, 2));
            int BirthDay = int.Parse(user.CNP.Substring(5, 2));

            try
            {
                birthDate = new DateTime(birthYear <= DateTime.Now.Year % 100 ?
                birthYear + 2000 : birthYear + 1900, birthMonth, BirthDay);
            }

            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid ID!\n");
                return Task.FromResult(new AddUserResponse() { Status = AddUserResponse.Types.Status.Error });
            }

            int age = (DateTime.Now.Month >= birthDate.Month && DateTime.Now.Day >= birthDate.Day ?
                DateTime.Now.Year - birthDate.Year : DateTime.Now.Year - birthDate.Year - 1);
            Console.WriteLine($"Age: {age}");

            string gender = (user.CNP.ElementAt(0)) switch
            {
                '1' or '5' => "Male",
                '2' or '6' => "Female",
                _ => "Other",
            };
            Console.WriteLine($"Gender: {gender}\n");

            return Task.FromResult(new AddUserResponse()
            { Status = AddUserResponse.Types.Status.Success, Name = user.Name, Gender = gender, Age = age });
        }
    }
}