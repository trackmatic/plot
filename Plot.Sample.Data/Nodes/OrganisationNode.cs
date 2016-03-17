namespace Plot.Sample.Data.Nodes
{
    public class OrganisationNode
    {
        public OrganisationNode()
        {
            
        }

        public OrganisationNode(Organisation item)
        {
            Id = item.Id;
            Name = item.Name;
            RegistrationNo = item.RegistrationNo;
            LegalName = item.LegalName;
            Timezone = item.RegionalSettings.Timezone;
            CurrencySymbol = item.RegionalSettings.CurrencySymbol;
            DateFormat = item.RegionalSettings.DateFormat;
            TimeFormat = item.RegionalSettings.TimeFormat;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string RegistrationNo { get; set; }

        public string LegalName { get; set; }

        public string Timezone { get; set; }

        public string CurrencySymbol { get; set; }

        public string DateFormat { get; set; }

        public string TimeFormat { get; set; }

        public Organisation AsOrganisation()
        {
            var organisation = new Organisation
            {
                Id = Id,
                Name = Name,
                LegalName = LegalName,
                RegistrationNo = RegistrationNo,
                RegionalSettings = new RegionalSettings
                {
                    Timezone = Timezone,
                    CurrencySymbol = CurrencySymbol,
                    DateFormat = DateFormat,
                    TimeFormat = TimeFormat
                }
            };
            return organisation;
        }
    }
}
