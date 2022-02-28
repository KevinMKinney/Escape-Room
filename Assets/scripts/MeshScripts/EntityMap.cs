using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMap {

    public static float[,] generateEntityMap(float[,] noiseMap, float[,] densityMap, float thresh, float spread) {
        //float[,] threshNoise = noiseMap.Where(x => (x > thresh)).ToArray();
        float[,] validEntityLocations = getValidLocations(densityMap, thresh);

        return validEntityLocations;
    }

    private static float[,] getValidLocations(float[,] densityMap, float thresh) {
        int mapWidth = densityMap.GetLength(0);
        int mapHeight = densityMap.GetLength(1);

        float[,] validLocations = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                validLocations[x, y] = System.Convert.ToSingle(densityMap[x, y] >= thresh);
            }
        }

        return validLocations;
    }

}
