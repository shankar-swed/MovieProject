using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace CodWizardsMovieShop.Models.Db
{
    public class Movie
    {
        public int Id { get; set; }

        [StringLength(50)]

        public string Title { get; set; }=string.Empty; 

        [StringLength(50)]

        public string Director { get; set; }=string.Empty;


        [DisplayName("Movie URL")]
        public string MovieUrl { get; set; }=string.Empty;

        [DisplayName("Realease Year")]
        public int ReleaseYear { get; set; }

        public double Price  { get; set; }



        public List<OrderRow> OrderRows { get; set; } = new List<OrderRow>();

    }
}
