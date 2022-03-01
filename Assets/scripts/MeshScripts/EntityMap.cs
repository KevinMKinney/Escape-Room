using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMap {

    public static float[,] generateEntityMap(float[,] noiseMap, float[,] densityMap, float minThresh, float maxThresh, float frequency) {
        //float[,] threshNoise = noiseMap.Where(x => (x > thresh)).ToArray();
        if (maxThresh < minThresh) {
            maxThresh = minThresh;
        }

        float[,] validEntityLocations = getValidLocations(noiseMap, minThresh, maxThresh);

        validEntityLocations = calculateGroups(validEntityLocations, densityMap, frequency);

        return validEntityLocations;
    }

    private static float[,] getValidLocations(float[,] noiseMap, float minThresh, float maxThresh) {
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        float[,] validLocations = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                validLocations[x, y] = System.Convert.ToSingle((noiseMap[x, y] >= minThresh) && (noiseMap[x, y] <= maxThresh));
            }
        }

        return validLocations;
    }

    private static float[,] calculateGroups(float[,] entityLocations, float[,] densityMap, float frequency) {
        int mapWidth = entityLocations.GetLength(0);
        int mapHeight = entityLocations.GetLength(1);

        float[,] groups = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                groups[x, y] = entityLocations[x, y] * densityMap[x, y] * frequency;
            }
        }

        return groups;
    }

}
