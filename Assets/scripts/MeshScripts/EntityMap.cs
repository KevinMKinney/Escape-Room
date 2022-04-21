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

        //int points = (int)(mapWidth*mapHeight / spread);
        //int limit = 10;
        //float[,] randomSpreadPoints = getRandomSpreadPoints(mapWidth, mapHeight, seed, spread, points, limit);
        //validEntityLocations = multiplyMaps(validEntityLocations, randomSpreadPoints, 1);
        //return validEntityLocations;

        //float[,] randomSpreadPoints = randomSpreadPoints(mapWidth, mapHeight, validEntityLocations, seed, spread);
        return randomSpreadPoints(mapWidth, mapHeight, validEntityLocations, seed, spread);
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
    private static float[,] getRandomSpreadPoints(int mapWidth, int mapHeight, int seed, float spread, int points, int limit) {
        //Debug.Log("X: "+randX+" | Y: "+randY);

        if (mapWidth <= 1 || mapHeight <= 1 || spread < 0 || points < 1 || (points*spread) >= (mapWidth*mapHeight)) {
            throw new Exception();
        }

        float[,] spreadPoints = new float[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                spreadPoints[x,y] = 0;
            }
        }

        System.Random prng = new System.Random(seed);
        bool redo;
        int randX;
        int randY;
        Vector2 point;
        Vector2[] invalidPoints;
        Vector2 invalidPoint;
        int lim = 0;

        while (points > 0) {
            do {
                redo = false;
                randX = prng.Next(0, mapWidth);
                randY = prng.Next(0, mapHeight);
                point = new Vector2(randX, randY);
                //Debug.Log("Testing point: "+randX+", "+randY);
            //} while (spreadPoints[(int)point.x, (int)point.y] != 0);
                invalidPoints = getPointsInRadius(mapWidth, mapHeight, point, spread);

                for (int i = 0; i < invalidPoints.Length; i++) {
                    invalidPoint = invalidPoints[i];
                    if (spreadPoints[(int)invalidPoint.x, (int)invalidPoint.y] == 1) {
                        redo = true;
                    }
                }
                lim++;

            } while (redo && (lim > limit));

            if (lim <= limit) {
                spreadPoints[(int)point.x, (int)point.y] = 1;
            }
            lim = 0;
            points--;
        }

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

        //Debug.Log("minX: "+minX+" | maxX: "+maxX+" | minY: "+minY+" | maxY: "+maxY);

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

    private static float[,] randomSpreadPoints(int mapWidth, int mapHeight, float[,] points, int seed, float spread) {

        List<float[]> spreadPoints = new List<float[]>();
        float[] temp = new float[2];
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                if (points[x, y] >= 1) {
                    temp[0] = x;
                    temp[1] = y;
                    //Debug.Log("temp: ("+x+", "+y+")");
                    spreadPoints.Add(temp);

                }
            }
        }
        /*
        for (int j = 0; j < spreadPoints.Count; j++) {
            Debug.Log("val ("+spreadPoints.ElementAt(j)[0]+", "+spreadPoints.ElementAt(j)[0]+") at "+j);
        }*/

        float[,] randomSpread = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);
        int size = spreadPoints.Count;
        float[] item = new float[2];
        int rng;
        double dist;

        while (size > 0) {

            rng = prng.Next(0, size);
            //Debug.Log("Size: "+size+" | rng#: "+rng);
            for (int i = 0; i < size;) {
                if (i != rng) {
                    //Debug.Log("I: "+i+" | rng: "+rng);
                    temp = spreadPoints[i];
                    item = spreadPoints[rng];
                    //Debug.Log("("+item[0]+" - "+temp[0]+") + ("+item[1]+" - "+temp[1]+")");
                    dist = Math.Sqrt((item[0] - temp[0]) + (item[1] - temp[1]));
                    //Debug.Log("dist("+dist+") < spread("+spread+")");
                    if (dist < spread) {
                        spreadPoints.RemoveAt(i);
                        size--;
                        rng--;
                    } else {
                         i++;
                    }
                } else {
                    i++;
                }
            }

            randomSpread[(int)item[0], (int)item[1]] = 1;
            spreadPoints.RemoveAt(rng);
            size--;

        }

        return randomSpread;
    }
}
