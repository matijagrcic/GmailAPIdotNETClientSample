using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

namespace GmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().ListLabels().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }

            Console.ReadKey();
        }

        public async Task ListLabels()
        {

            UserCredential credential;
            using (var stream = new FileStream("client_secrets_desktop.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    new[] { GmailService.Scope.GmailReadonly },
                    "user", CancellationToken.None);
            }
          
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Gmail Test",
            });

            try
            {
                ListLabelsResponse response = service.Users.Labels.List("me").Execute();
                foreach (Label label in response.Labels.OrderBy(p=>p.Name))
                {
                    Console.WriteLine(label.Id + " - " + label.Name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
        
    }
}
