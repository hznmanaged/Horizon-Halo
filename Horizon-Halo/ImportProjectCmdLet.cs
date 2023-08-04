using System;
using System.Management.Automation;
using Horizon.Halo;
using Horizon.Halo.Services;
using System.IO;

namespace Horizon_Halo
{
    [Cmdlet(verbName: "Import", nounName: "MicrosoftProject")]
    public class ImportProjectCmdLet : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int TargetClientID { get; set; }


        [Parameter(Position = 2, Mandatory = true)]
        public string URL { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string APIClientID { get; set; }

        [Parameter(Position = 4, Mandatory = true)]
        public string APIClientSecret { get; set; }

        protected override void EndProcessing()
        {
            try
            {
                var client = new HaloAPIClient(url: URL, clientID: APIClientID, clientSecret: APIClientSecret);

                var importer = new ProjectImportService();

                using(var stream = File.OpenRead(Path))
                {
                    importer.Import(input: stream, halo: client, clientId: TargetClientID).GetAwaiter().GetResult();
                }

            }
            catch(Exception ex)
            {
                this.ThrowTerminatingError(new ErrorRecord(ex, "1", ErrorCategory.NotSpecified, this));
            }
            base.EndProcessing();
        }
    }
}
