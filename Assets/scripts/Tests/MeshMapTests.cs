using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class MeshMapTest {
    /*
    // Tests for generateVerticies function
    [Test]
    public void generateVerticiesValidInputs() {
        float[,] testNoise = new float[,] {{0,1},{2,3}};
        Vector3[] testing = MeshMap.generateVerticies(testNoise);
        Vector3[] correct = new Vector3[4];
        correct[0] = new Vector3(0,0,0);
        correct[1] = new Vector3(1,2,0);
        correct[2] = new Vector3(0,1,1);
        correct[3] = new Vector3(1,3,1);
        Assert.AreEqual(correct, testing);

        testNoise = new float[,] {{0,0.5f,1},{0.5f,1,1.5f}};
        testing = MeshMap.generateVerticies(testNoise);
        correct = new Vector3[6];
        correct[0] = new Vector3(0,0,0);
        correct[1] = new Vector3(1,0.5f,0);
        correct[2] = new Vector3(0,0.5f,1);
        correct[3] = new Vector3(1,1,1);
        correct[4] = new Vector3(0,1,2);
        correct[5] = new Vector3(1,1.5f,2);
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateVerticiesInvalidInputs() {
        float[,] testNoise = new float[0,0];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
        testNoise = new float[1,0];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
        testNoise = new float[0,1];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
        testNoise = new float[1,1];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
    }

    // Tests for generateTriangles function
    [Test]
    public void generateTrianglesValidInputs() {
        int[] testing = MeshMap.generateTriangles(3,2);
        int[] correct = new int[] {0,3,1,1,3,4,1,4,2,2,4,5};
        Assert.AreEqual(correct, testing);

        testing = MeshMap.generateTriangles(4,3);
        correct = new int[] {0,4,1,1,4,5,1,5,2,2,5,6,2,6,3,3,6,7,4,8,5,5,8,9,5,9,6,6,9,10,6,10,7,7,10,11};
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateTrianglesInvalidInputs() {
        Assert.That(() => MeshMap.generateTriangles(3,1), Throws.TypeOf<Exception>());
        Assert.That(() => MeshMap.generateTriangles(1,1), Throws.TypeOf<Exception>());
    }
    */

    // Black box tests:
    // Tests for generateVerticies function
    [Test]
    public void generateVerticiesValidIntInput() {
        float[,] testNoise = new float[,] {{0,1},{2,3}};
        Vector3[] testing = MeshMap.generateVerticies(testNoise);
        Vector3[] correct = new Vector3[4];
        correct[0] = new Vector3(0,0,0);
        correct[1] = new Vector3(1,2,0);
        correct[2] = new Vector3(0,1,1);
        correct[3] = new Vector3(1,3,1);
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateVerticiesValidFloatInput() {
        float[,] testNoise = new float[,] {{0,0.5f,1},{0.5f,1,1.5f}};
        Vector3[] testing = MeshMap.generateVerticies(testNoise);
        Vector3[] correct = new Vector3[6];
        correct[0] = new Vector3(0,0,0);
        correct[1] = new Vector3(1,0.5f,0);
        correct[2] = new Vector3(0,0.5f,1);
        correct[3] = new Vector3(1,1,1);
        correct[4] = new Vector3(0,1,2);
        correct[5] = new Vector3(1,1.5f,2);
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateVerticiesInvalidInput00() {
        float[,] testNoise = new float[0,0];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
    }

    [Test]
    public void generateVerticiesInvalidInput10() {
        float[,] testNoise = new float[1,0];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());

    }

    [Test]
    public void generateVerticiesInvalidInput01() {
        float[,] testNoise = new float[0,1];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());

    }

    [Test]
    public void generateVerticiesInvalidInput11() {
        float[,] testNoise = new float[1,1];
        Assert.That(() => MeshMap.generateVerticies(testNoise), Throws.TypeOf<Exception>());
    }

    // Tests for generateTriangles function
    [Test]
    public void generateTrianglesValidInput() {
        int[] testing = MeshMap.generateTriangles(3,2);
        int[] correct = new int[] {0,3,1,1,3,4,1,4,2,2,4,5};
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateTrianglesValidInputBackup() {
        int[] testing = MeshMap.generateTriangles(4,3);
        int[] correct = new int[] {0,4,1,1,4,5,1,5,2,2,5,6,2,6,3,3,6,7,4,8,5,5,8,9,5,9,6,6,9,10,6,10,7,7,10,11};
        Assert.AreEqual(correct, testing);
    }

    [Test]
    public void generateTrianglesInvalidInput10() {
        Assert.That(() => MeshMap.generateTriangles(1,3), Throws.TypeOf<Exception>());

    }

    [Test]
    public void generateTrianglesInvalidInput01() {
        Assert.That(() => MeshMap.generateTriangles(3,1), Throws.TypeOf<Exception>());

    }

    [Test]
    public void generateTrianglesInvalidInput11() {
        Assert.That(() => MeshMap.generateTriangles(1,1), Throws.TypeOf<Exception>());
    }

    // White box tests:
    // Tests for generateColors function
    [Test]
    // achieves full condition coverage
    public void generateColorsTest() {
        Mesh meshTest = new Mesh();
        meshTest.name = "TestMesh";
        meshTest.Clear();

        float[,] testNoise = new float[,] {{-1,-1,0,0,0,1,1},{-1,-1,0,0,0,1,1}};
        int mapWidth = testNoise.GetLength(0);
        int mapHeight = testNoise.GetLength(1);

        float waterThresh = -0.5f;
        float snowThresh = 0.5f;

        meshTest.SetVertices(MeshMap.generateVerticies(testNoise));
        meshTest.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        meshTest.RecalculateNormals();

        Color col1 = Color.green;
        Color col2 = new Color32(160, 82, 45, 1);
        Color col3 = Color.white;

        float[] steepVal = MeshMap.calculateSteepness(meshTest, mapWidth, mapHeight);
        Color[] meshCols = MeshMap.generateColors(meshTest, mapWidth, mapHeight, steepVal, snowThresh, waterThresh, col1, col2);

        Assert.IsTrue((meshCols[0] == col2) && (meshCols[1] == col2) && (meshCols[6] == col1) && (meshCols[7] == col1) && (meshCols[12] == col3) && (meshCols[13] == col3));
    }
}
