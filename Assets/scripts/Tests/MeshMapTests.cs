using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class MeshMapTest
{
    // Tests for generateVerticies functions
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

    // Tests for generateTriangles functions
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



}
