using System.ComponentModel;

namespace FhatFinder.Shared.Filters
{
    /*
     * filter_profession=0&
     * filter_levelrangefrom=100&
     * filter_levelrangeto=600&
     * filter_world=&
     * filter_worldpvptype=0&
     * filter_worldbattleyestate=2&
     * filter_skillid=0&
     * filter_skillrangefrom=0&
     * filter_skillrangeto=0&
     * order_column=0&
     * order_direction=0&
     * searchtype=1&
     * currentpage=0
     */

    public abstract class AuctionFilterBase : IAuctionFilter
    {
        [DisplayName("filter_profession")]
        public Vocation Vocation { get; set; } = Vocation.All;

        [DisplayName("filter_levelrangefrom")]
        public int LevelRangeFrom { get; set; }
        [DisplayName("filter_levelrangeto")]
        public int LevelRangeTo { get; set; }

        [DisplayName("filter_world")]
        public string World { get; set; }

        [DisplayName("filter_worldpvptype")]
        public PvP PvPType { get; set; } = PvP.All;

        [DisplayName("filter_worldbattleyestate")]
        public BattlEye BattlEyeState { get; set; } = BattlEye.All;

        [DisplayName("filter_skillid")]
        public Skill Skill { get; set; } = Skill.None;
        [DisplayName("filter_skillrangefrom")]
        public int SkillRangeFrom { get; set; }
        [DisplayName("filter_skillrangeto")]
        public int SkillRangeTo { get; set; }

        [DisplayName("order_column")]
        public OrderBy OrderBy { get; set; } = OrderBy.EndDate;
        [DisplayName("order_direction")]
        public OrderByDirection OrderByDirection { get; set; } = OrderByDirection.HighestLatest;

        [DisplayName("searchtype")]
        public int SearchType { get; set; } = 1;

        [DisplayName("currentpage")]
        public int CurrentPage { get; set; } = 1;
    }
}
