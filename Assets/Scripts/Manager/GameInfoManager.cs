using UnityEngine;
using System.Collections;

public class GameInfoManager
{
    public static string fileName = "gameInfo";

    public static GameInfoObject gameInfo;

    public static string[] stringSeparators = new string[] { "Resources/" };

    public static void SaveGameInfo()
    {
        gameInfo.Save(fileName + "save");
    }

    public static void LoadGameInfo()
    {
        gameInfo = new GameInfoObject();
        gameInfo.Load(fileName + "save");
    }

    public static CharacterObject getCharacter(string characterName)
    {
        CharacterObject cO = gameInfo.characters[0];

        foreach (CharacterObject c in gameInfo.characters)
        {
            if (c.name == characterName)
            {
                cO = c;
            }
        }

        return cO;
    }

    public static SequenceObject getSequence(string sequenceName)
    {
        SequenceObject sO = gameInfo.sequences[0];

        foreach (SequenceObject s in gameInfo.sequences)
        {
            if (s.id == sequenceName)
            {
                sO = s;
            }
        }

        return sO;
    }

    public static ExplorationObject getExploration(string explorationName)
    {
        ExplorationObject eO = gameInfo.explorations[0];

        foreach (ExplorationObject e in gameInfo.explorations)
        {
            if (e.name == explorationName)
            {
                eO = e;
            }
        }

        return eO;
    }

    public static PlaceObject getPlace(string placeName)
    {
        PlaceObject pO = gameInfo.places[0];

        foreach (PlaceObject p in gameInfo.places)
        {
            if (p.name == placeName)
            {
                pO = p;
            }
        }

        return pO;
    }

}
