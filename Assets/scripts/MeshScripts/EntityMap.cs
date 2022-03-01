using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMap {

    public static float[,] generateEntityMap(float[,] noiseMap, float[,] densityMap, int seed, float minThresh, float maxThresh, float frequency, float spread) {

        // check for invalid input(s)
        if (maxThresh < minThresh) {
            maxThresh = minThresh;
        }

        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        // get valid locations
        float[,] validEntityLocations = getValidLocations(noiseMap, minThresh, maxThresh);
        // make groups of entities
        validEntityLocations = multiplyMaps(validEntityLocations, densityMap, frequency);

        //float[,] randomSpreadPoints = getRandomSpreadPoints(mapWidth, mapHeight, seed, spread);

        return validEntityLocations;
    }

    // finds all valid locations for entities from the mesh's noise
    private static float[,] getValidLocations(float[,] noiseMap, float minThresh, float maxThresh) {
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        float[,] validLocations = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                // index is either 1 (valid) or 0 (not valid)
                validLocations[x, y] = System.Convert.ToSingle((noiseMap[x, y] >= minThresh) && (noiseMap[x, y] <= maxThresh));
            }
        }

        return validLocations;
    }

    // ultility function that multiplies indexes of a 2d float array
    // (would proably be better to do it functionally, but I'm not sure how to with multidimensional arrays)
    private static float[,] multiplyMaps(float[,] a, float[,] b, float magnitude) {
        int mapWidth = a.GetLength(0);
        int mapHeight = a.GetLength(1);

        float[,] result = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                result[x, y] = a[x, y] * b[x, y] * magnitude;
            }
        }

        return result;
    }

    // WIP
    private static float[,] getRandomSpreadPoints(int mapWidth, int mapHeight, int seed, float spread) {

        System.Random prng = new System.Random(seed);
        int randX = prng.Next(0, mapWidth);
        int randY = prng.Next(0, mapHeight);

        float[,] spreadPoints = new float[mapWidth, mapHeight];
        //Vector2 direction = new Vector2[prng.Next(-1, 1), prng.Next(-1, 1)];

        return spreadPoints;
    }
}
