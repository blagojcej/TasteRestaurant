namespace TasteRestaurant.Data
{
    public class IndividualButtonPartial
    {
        public string Page { get; set; }
        public string Glyph { get; set; }
        public string ButtonType { get; set; }

        public int? Id { get; set; }

        public string ActionParameters
        {
            get
            {
                if (Id.HasValue && Id != 0)
                {
                    return Id.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
