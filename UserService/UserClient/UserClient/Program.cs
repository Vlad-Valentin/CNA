using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UserClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UserManager.UserManagerClient(channel);

            var cancellationToken = new CancellationTokenSource(Timeout.Infinite);

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();
                Console.Write("CNP: ");
                var cnp = Console.ReadLine();

                var userToBeAdded = new User() { Name = name.Trim().Length > 0 ? name : "Invalid Name", CNP = cnp.Trim().Length == 13 ? cnp : "Invalid ID" };
                var response = await client.AddUserAsync(new AddUserRequest { User = userToBeAdded });

                if (response.Status == AddUserResponse.Types.Status.Success)
                {
                    Console.WriteLine($"\nResponse Status: {response.Status}\n\nName: {response.Name}\nGender: {response.Gender}\nAge: {response.Age}\n");
                }
                else if (response.Name.Equals("Invalid Name"))
                {
                    Console.WriteLine($"\nResponse Status: {response.Status}\nInvalid Name!\n");
                }
                else
                {
                    Console.WriteLine($"\nResponse Status: {response.Status}\nInvalid ID!\n");
                }
            }
        }
    }
}