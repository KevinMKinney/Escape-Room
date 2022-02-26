using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    /*
     * code based on Sebastian Lague's tutorial (https://www.youtube.com/watch?v=MRNFcywkUSA)
     * mapWidth & mapHeight are for the size of the noise map
     * seed is (basically) the id for that specific Perlin noise
     * scale is for changing the size of the Perlin noise
     * octaves is for the amount of noise maps
     * lacunarity controls the increase of frequency between octaves
     * persistance controls the decrease in amplitude of octaves
     * offset is just to move noise map on texture
    */
    public static float[,] generateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, AnimationCurve heightCurve, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // if scale is <=0, unity will crash
        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x-halfWidth) / scale * frequency + octavesOffsets[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octavesOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2-1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // normalize noise map
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = heightCurve.Evaluate(Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]));
            }
        }

        return noiseMap;
    }

    public static float[,] generateFlushNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {

      float[,] noiseMap = new float[mapWidth, mapHeight];

      System.Random prng = new System.Random(seed);
      Vector2[] octavesOffsets = new Vector2[octaves];
      for(int i = 0; i < octaves; i++) {
          float offsetX = prng.Next(-100000, 100000) + offset.x;
          float offsetY = prng.Next(-100000, 100000) + offset.y;
          octavesOffsets[i] = new Vector2(offsetX, offsetY);
      }

      // if scale is <=0, unity will crash
      if (scale <= 0) {
          scale = 0.0001f;
      }

      float maxNoiseHeight = float.MinValue;
      float minNoiseHeight = float.MaxValue;

      float halfWidth = mapWidth / 2;
      float halfHeight = mapHeight / 2;

      for (int y = 0; y < halfHeight; y++) {
          for (int x = 0; x < halfWidth; x++) {
              float amplitude = 1;
              float frequency = 1;
              float noiseHeight = 0;

              for (int i = 0; i < octaves; i++) {
                  float sampleX = (x-(halfWidth/2)) / scale * frequency + octavesOffsets[i].x;
                  float sampleY = (y-(halfHeight/2)) / scale * frequency + octavesOffsets[i].y;

                  float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2-1;
                  noiseHeight += perlinValue * amplitude;

                  amplitude *= persistance;
                  frequency *= lacunarity;
              }

              if (noiseHeight > maxNoiseHeight) {
                  maxNoiseHeight = noiseHeight;
              } else if (noiseHeight < minNoiseHeight) {
                  minNoiseHeight = noiseHeight;
              }

              noiseMap[x, y] = noiseHeight; //TL
              noiseMap[(mapWidth-1-x), y] = noiseHeight; //TR
              noiseMap[x, (mapHeight-1-y)] = noiseHeight; //BL
              noiseMap[(mapWidth-1-x), (mapHeight-1-y)] = noiseHeight; //BR
          }
      }

      // normalize noise map
      for (int y = 0; y < mapHeight; y++) {
          for (int x = 0; x < mapWidth; x++) {
              noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
          }
      }

      return noiseMap;
      }
}
