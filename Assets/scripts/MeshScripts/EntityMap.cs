using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        int points = 250;
        float[,] randomSpreadPoints = getRandomSpreadPoints(mapWidth, mapHeight, seed, spread, points);
        validEntityLocations = multiplyMaps(validEntityLocations, randomSpreadPoints, 1);

        return validEntityLocations;
    }

    // finds all valid locations for entities from the mesh's noise
    private static float[,] getValidLocations(float[,] noiseMap, float minThresh, float maxThresh) {
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        if (mapWidth <= 1 || mapHeight <= 1) {
            throw new Exception();
            //return null;
        }

        if (minThresh > maxThresh) {
            throw new Exception();
        }

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

    //
    private static float[,] getRandomSpreadPoints(int mapWidth, int mapHeight, int seed, float spread, int points) {
        //Debug.Log("X: "+randX+" | Y: "+randY);

        float[,] spreadPoints = new float[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                spreadPoints[x,y] = 0;
            }
        }

        if (points >= (mapWidth * mapHeight)) {
            Debug.Log("Invalid number of points");
            return spreadPoints;
        }

        System.Random prng = new System.Random(seed);
        Vector2 point;
        while (points > 0) {
            do {
                int randX = prng.Next(0, mapWidth);
                int randY = prng.Next(0, mapHeight);
                point = new Vector2(randX, randY);
                //Debug.Log("Testing point: "+randX+", "+randY);
            } while (spreadPoints[(int)point.x, (int)point.y] != 0);

            Vector2[] invalidPoints = getPointsInRadius(mapWidth, mapHeight, point, spread);
            int invalidSize = invalidPoints.Length;
            for (int i = 0; i < invalidSize; i++) {
                Vector2 invalidPoint = invalidPoints[i];
                //Debug.Log("test: "+spreadPoints[(int)invalidPoint.x, (int)invalidPoint.y]);
                spreadPoints[(int)invalidPoint.x, (int)invalidPoint.y] = 0;
            }

            spreadPoints[(int)point.x, (int)point.y] = 1;

            points--;
        }
        /*
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                Debug.Log("spreadPoints: "+spreadPoints[x, y]);
            }
        }*/

        return spreadPoints;
    }

    private static Vector2[] getPointsInRadius(int mapWidth, int mapHeight, Vector2 point, float radius) {
        int index = 0;
        Vector2[] pointArray = new Vector2[0];

        int radFloor = Mathf.FloorToInt(radius);
        int radCeil = Mathf.CeilToInt(radius);

        int minX = Math.Clamp(((int)point.x - radFloor), 0, mapWidth);
        int maxX = Math.Clamp(((int)point.x + radCeil), 0, mapWidth);
        int minY = Math.Clamp(((int)point.y - radFloor), 0, mapHeight);
        int maxY = Math.Clamp(((int)point.y + radCeil), 0, mapHeight);

        for (int y = minY; y < maxY; y++) {
            for (int x = minX; x < maxX; x++) {
                Vector2 other = new Vector2(x,y);
                float dist = Vector2.Distance(point, other);
                if (dist <= radius) {
                    Array.Resize(ref pointArray, pointArray.Length+1);
                    pointArray[index] = other;
                    index++;
                }
            }
        }

        return pointArray;
    }
}
