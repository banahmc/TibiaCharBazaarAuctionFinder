namespace FhatFinder.Shared.Filters
{
    public interface IAuctionFilter
    {
        Vocation Vocation { get; set; }
        int LevelRangeFrom { get; set; }
        int LevelRangeTo { get; set; }
        string World { get; set; }
        PvP PvPType { get; set; }
        BattlEye BattlEyeState { get; set; }
        Skill Skill { get; set; }
        int SkillRangeFrom { get; set; }
        int SkillRangeTo { get; set; }
        OrderBy OrderBy { get; set; }
        OrderByDirection OrderByDirection { get; set; }
        public int SearchType { get; set; }
        public int CurrentPage { get; set; }
    }
}
