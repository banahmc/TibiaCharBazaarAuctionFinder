using System.ComponentModel.DataAnnotations;

namespace FhatFinder.Shared
{
    public enum PvP
    {
        All = 9,
        Open = 0,
        Optional = 1,
        Hardcore = 2,
        RetroOpen = 3,
        RetroHardcore = 4
    }

    public enum BattlEye
    {
        All = 0,
        InitiallyProtected,
        Protected,
        NotProtected
    }

    public enum Vocation
    {
        All = 0,
        None,
        Druid,
        Knight,
        Paladin,
        Sorcerer
    }

    public enum Skill
    {
        None = 0,
        MagicLevel = 1,
        Shielding = 6,
        DistanceFighting = 7,
        SwordFighting = 8,
        ClubFighting = 9,
        AxeFighting = 10,
        FistFighting = 11,
        Fishing = 13
    }

    public enum OrderBy
    {
        Bid = 100,
        EndDate = 101,
        StartDate = 103,
        CharacterLevel = 102,
        MagicLevel = 1,
        Shielding = 6,
        DistanceFighting = 7,
        SwordFighting = 8,
        ClubFighting = 9,
        AxeFighting = 10,
        FistFighting = 11,
        Fishing = 13
    }

    public enum OrderByDirection
    {
        HighestLatest = 0,
        LowestEarliest
    }

    // TODO: Add other outfits
    public enum Outfit
    {
        Unknown = 0,
        [Display(Name = "Citizen")]
        MaleCitizen = 128,
        [Display(Name = "Hunter")]
        MaleHunter = 129,
        [Display(Name = "Mage")]
        MaleMage = 130,
        [Display(Name = "Knight")]
        MaleKnight = 131,
        [Display(Name = "Summoner")]
        FemaleSummoner = 141,
    }

    public enum Addon
    {
        None = 0,
        First,
        Second,
        FirstAndSecond
    }
}
