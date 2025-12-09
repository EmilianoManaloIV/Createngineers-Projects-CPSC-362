using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// Unit and Integration tests for UNO Game.
// Designed for the Unity Test Runner.
public class UnoGameTests
{
    private GameManager gameManager;
    private GameObject gameManagerObject;

    // Setup: Runs before every test to ensure a clean environment
    [SetUp]
    public void Setup()
    {
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();
        
        // Mock data initialization (Simplified for testing)
        gameManager.topCard = ScriptableObject.CreateInstance<CardData>();
        gameManager.topCard.cardColor = CardColor.Red;
        gameManager.topCard.cardType = CardType.Five;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameManagerObject);
    }

    // --- Function 1: IsValidPlay Tests ---

    [Test]
    public void IsValidPlay_SameColor_ReturnsTrue()
    {
        // Arrange
        CardData playCard = ScriptableObject.CreateInstance<CardData>();
        playCard.cardColor = CardColor.Red;
        playCard.cardType = CardType.Skip; // Different type, same color

        // Act
        bool result = gameManager.IsValidPlay(playCard);

        // Assert
        Assert.IsTrue(result, "Card of the same color should be playable.");
    }

    [Test]
    public void IsValidPlay_DifferentColorAndType_ReturnsFalse()
    {
        // Arrange
        CardData playCard = ScriptableObject.CreateInstance<CardData>();
        playCard.cardColor = CardColor.Blue;
        playCard.cardType = CardType.Reverse; 
        // Top card is Red Number

        // Act
        bool result = gameManager.IsValidPlay(playCard);

        // Assert
        Assert.IsFalse(result, "Card with different color and type should NOT be playable.");
    }
    
    [Test]
    public void IsValidPlay_WildCard_ReturnsTrue()
    {
        // Arrange
        CardData playCard = ScriptableObject.CreateInstance<CardData>();
        playCard.cardType = CardType.Wild;

        // Act
        bool result = gameManager.IsValidPlay(playCard);

        // Assert
        Assert.IsTrue(result, "Wild card should always be playable.");
    }

    // --- Function 2: Uno Visibility Logic Tests ---
    
    [Test]
    public void UpdateUnoButton_TwoCardsOnePlayable_ReturnsTrue()
    {
        // Arrange
        List<CardData> mockHand = new List<CardData>();
        
        CardData unplayable = ScriptableObject.CreateInstance<CardData>();
        unplayable.cardColor = CardColor.Blue; // Top is Red
        
        CardData playable = ScriptableObject.CreateInstance<CardData>();
        playable.cardColor = CardColor.Red; // Matches Top
        
        mockHand.Add(unplayable);
        mockHand.Add(playable);

        // Act
        // Logic replicated from GameManager fix
        bool showButton = false;
        if (mockHand.Count == 2)
        {
            foreach(var c in mockHand)
            {
                // We use the helper we tested above
                if(gameManager.IsValidPlay(c)) 
                {
                    showButton = true;
                    break;
                }
            }
        }

        // Assert
        Assert.IsTrue(showButton, "Uno button should show if player has 2 cards and 1 is playable.");
    }

    [Test]
    public void UpdateUnoButton_TwoCardsNonePlayable_ReturnsFalse()
    {
        // Arrange
        List<CardData> mockHand = new List<CardData>();
        
        CardData unplayable1 = ScriptableObject.CreateInstance<CardData>();
        unplayable1.cardColor = CardColor.Blue; 
        
        CardData unplayable2 = ScriptableObject.CreateInstance<CardData>();
        unplayable2.cardColor = CardColor.Yellow; 
        
        mockHand.Add(unplayable1);
        mockHand.Add(unplayable2);

        // Act
        bool showButton = false;
        if (mockHand.Count == 2)
        {
            foreach(var c in mockHand)
            {
                if(gameManager.IsValidPlay(c)) 
                {
                    showButton = true;
                    break;
                }
            }
        }

        // Assert
        Assert.IsFalse(showButton, "Uno button should NOT show if player cannot actually play a card.");
    }

    // --- Function 3: Turn Logic Tests ---

    [Test]
    public void GetNextPlayer_NormalRotation_IncrementsIndex()
    {
        // Arrange
        // Using Reflection to set private fields for testing logic
        System.Reflection.FieldInfo indexField = typeof(GameManager).GetField("currentPlayerIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        indexField.SetValue(gameManager, 0); // Current player 0
        
        System.Reflection.FieldInfo playersField = typeof(GameManager).GetField("players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        playersField.SetValue(gameManager, new List<Player>(new Player[4])); // Mock 4 players

        // Act
        // Use Reflection to invoke private method 'AdvancePlayerIndex'
        System.Reflection.MethodInfo advanceMethod = typeof(GameManager).GetMethod("AdvancePlayerIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        advanceMethod.Invoke(gameManager, null);

        // Assert
        int newIndex = (int)indexField.GetValue(gameManager);
        Assert.AreEqual(1, newIndex, "Index should increment from 0 to 1 in normal rotation.");
    }
}