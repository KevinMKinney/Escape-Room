using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class EntityMapTest {

    // Black box tests:
    // Tests for getValidLocations function
    [Test]
    public void getValidLocationsValidTest() {
        float[,] testNoise = new float[,] {{0,1,0},{0.2f,0.8f,0.2f}};
        float[,] correct = new float[,] {{0,1,0},{0,1,0}};
        float[,] testLocations = EntityMap.getValidLocations(testNoise, 0.5f, 1);
        Assert.AreEqual(correct, testLocations);
    }

    [Test]
    public void getValidLocationsInvalidTest00() {
        float[,] testNoise = new float[0,0];
        Assert.That(() => EntityMap.getValidLocations(testNoise, 0, 1), Throws.TypeOf<Exception>());
    }

    [Test]
    public void getValidLocationsInvalidTest01() {
        float[,] testNoise = new float[0,1];
        Assert.That(() => EntityMap.getValidLocations(testNoise, 0, 1), Throws.TypeOf<Exception>());
    }

    [Test]
    public void getValidLocationsInvalidTest10() {
        float[,] testNoise = new float[1,0];
        Assert.That(() => EntityMap.getValidLocations(testNoise, 0, 1), Throws.TypeOf<Exception>());
    }

    [Test]
    public void getValidLocationsInvalidTest11() {
        float[,] testNoise = new float[1,1];
        Assert.That(() => EntityMap.getValidLocations(testNoise, 0, 1), Throws.TypeOf<Exception>());
    }

    [Test]
    public void getValidLocationsInvalidTestThresh() {
        float[,] testNoise = new float[3,3];
        Assert.That(() => EntityMap.getValidLocations(testNoise, 1, 0), Throws.TypeOf<Exception>());
    }

    // Tests for multiplyMaps function
    [Test]
    public void multiplyMapsValidTest() {
        float[,] testArr1 = new float[,] {{0, 0.2f, 0.8f, 1},{0, 0.2f, 0.8f, 1}};
        float[,] testArr2 = new float[,] {{0.5f, 0.5f, 0.5f, 0.5f},{1, 1, 1, 1}};
        float[,] correct = new float[,] {{0, 0.1f, 0.4f, 0.5f},{0, 0.2f, 0.8f, 1}};
        Assert.AreEqual(correct, EntityMap.multiplyMaps(testArr1, testArr2, 1));
    }

    [Test]
    public void multiplyMapsValidMagnitudeTest() {
        float[,] testArr1 = new float[,] {{0, 0.2f, 0.8f, 1},{0, 0.2f, 0.8f, 1}};
        float[,] testArr2 = new float[,] {{0.5f, 0.5f, 0.5f, 0.5f},{1, 1, 1, 1}};
        float[,] correct = new float[,] {{0, 0.2f, 0.8f, 1},{0, 0.4f, 1.6f, 2}};
        Assert.AreEqual(correct, EntityMap.multiplyMaps(testArr1, testArr2, 2));
    }

    [Test]
    public void multiplyMapsInvalidWidthTest() {
        float[,] testArr1 = new float[,] {{1,1,1},{1,1,1}};
        float[,] testArr2 = new float[,] {{1,1},{1,1}};
        Assert.That(() => EntityMap.multiplyMaps(testArr1, testArr2, 1), Throws.TypeOf<Exception>());
    }

    [Test]
    public void multiplyMapsInvalidHeightTest() {
        float[,] testArr1 = new float[,] {{1,1,1},{1,1,1}};
        float[,] testArr2 = new float[,] {{1,1,1},{1,1,1},{1,1,1}};
        Assert.That(() => EntityMap.multiplyMaps(testArr1, testArr2, 1), Throws.TypeOf<Exception>());
    }
}
