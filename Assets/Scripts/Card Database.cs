using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();


    void Awake()
    {
                        // Id, Value, Color)
        cardList.Add(new Card(0, 0, "None"));

        // All Red Cards
        cardList.Add(new Card(1, 0, "Red"));
        cardList.Add(new Card(2, 1, "Red"));
        cardList.Add(new Card(3, 2, "Red"));
        cardList.Add(new Card(4, 3, "Red"));
        cardList.Add(new Card(5, 4, "Red"));
        cardList.Add(new Card(6, 5, "Red"));
        cardList.Add(new Card(7, 6, "Red"));
        cardList.Add(new Card(8, 7, "Red"));
        cardList.Add(new Card(9, 8, "Red"));
        cardList.Add(new Card(10, 9, "Red"));
        cardList.Add(new Card(11, 10, "Red"));
        cardList.Add(new Card(12, 11, "Red"));
        cardList.Add(new Card(13, 12, "Red"));
        cardList.Add(new Card(14, 1, "Red"));
        cardList.Add(new Card(15, 2, "Red"));
        cardList.Add(new Card(16, 3, "Red"));
        cardList.Add(new Card(17, 4, "Red"));
        cardList.Add(new Card(18, 5, "Red"));
        cardList.Add(new Card(19, 6, "Red"));
        cardList.Add(new Card(20, 7, "Red"));
        cardList.Add(new Card(21, 8, "Red"));
        cardList.Add(new Card(22, 9, "Red"));
        cardList.Add(new Card(23, 10, "Red"));
        cardList.Add(new Card(24, 11, "Red"));
        cardList.Add(new Card(25, 12, "Red"));

        // All Yellow Cards
        cardList.Add(new Card(26, 0, "Yellow"));
        cardList.Add(new Card(27, 1, "Yellow"));
        cardList.Add(new Card(28, 2, "Yellow"));
        cardList.Add(new Card(29, 3, "Yellow"));
        cardList.Add(new Card(30, 4, "Yellow"));
        cardList.Add(new Card(31, 5, "Yellow"));
        cardList.Add(new Card(32, 6, "Yellow"));
        cardList.Add(new Card(33, 7, "Yellow"));
        cardList.Add(new Card(34, 8, "Yellow"));
        cardList.Add(new Card(35, 9, "Yellow"));
        cardList.Add(new Card(36, 10, "Yellow"));
        cardList.Add(new Card(37, 11, "Yellow"));
        cardList.Add(new Card(38, 12, "Yellow"));
        cardList.Add(new Card(39, 1, "Yellow"));
        cardList.Add(new Card(40, 2, "Yellow"));
        cardList.Add(new Card(41, 3, "Yellow"));
        cardList.Add(new Card(42, 4, "Yellow"));
        cardList.Add(new Card(43, 5, "Yellow"));
        cardList.Add(new Card(44, 6, "Yellow"));
        cardList.Add(new Card(45, 7, "Yellow"));
        cardList.Add(new Card(46, 8, "Yellow"));
        cardList.Add(new Card(47, 9, "Yellow"));
        cardList.Add(new Card(48, 10, "Yellow"));
        cardList.Add(new Card(49, 11, "Yellow"));
        cardList.Add(new Card(50, 12, "Yellow"));

        // All Green Cards
        cardList.Add(new Card(51, 0, "Green"));
        cardList.Add(new Card(52, 1, "Green"));
        cardList.Add(new Card(53, 2, "Green"));
        cardList.Add(new Card(54, 3, "Green"));
        cardList.Add(new Card(55, 4, "Green"));
        cardList.Add(new Card(56, 5, "Green"));
        cardList.Add(new Card(57, 6, "Green"));
        cardList.Add(new Card(58, 7, "Green"));
        cardList.Add(new Card(59, 8, "Green"));
        cardList.Add(new Card(60, 9, "Green"));
        cardList.Add(new Card(61, 10, "Green"));
        cardList.Add(new Card(62, 11, "Green"));
        cardList.Add(new Card(63, 12, "Green"));
        cardList.Add(new Card(64, 1, "Green"));
        cardList.Add(new Card(65, 2, "Green"));
        cardList.Add(new Card(66, 3, "Green"));
        cardList.Add(new Card(67, 4, "Green"));
        cardList.Add(new Card(68, 5, "Green"));
        cardList.Add(new Card(69, 6, "Green"));
        cardList.Add(new Card(70, 7, "Green"));
        cardList.Add(new Card(71, 8, "Green"));
        cardList.Add(new Card(72, 9, "Green"));
        cardList.Add(new Card(73, 10, "Green"));
        cardList.Add(new Card(74, 11, "Green"));
        cardList.Add(new Card(75, 12, "Green"));

        // All Blue Cards
        cardList.Add(new Card(76, 0, "Blue"));
        cardList.Add(new Card(77, 1, "Blue"));
        cardList.Add(new Card(78, 2, "Blue"));
        cardList.Add(new Card(79, 3, "Blue"));
        cardList.Add(new Card(80, 4, "Blue"));
        cardList.Add(new Card(81, 5, "Blue"));
        cardList.Add(new Card(82, 6, "Blue"));
        cardList.Add(new Card(83, 7, "Blue"));
        cardList.Add(new Card(84, 8, "Blue"));
        cardList.Add(new Card(85, 9, "Blue"));
        cardList.Add(new Card(86, 10, "Blue"));
        cardList.Add(new Card(87, 11, "Blue"));
        cardList.Add(new Card(88, 12, "Blue"));
        cardList.Add(new Card(89, 1, "Blue"));
        cardList.Add(new Card(90, 2, "Blue"));
        cardList.Add(new Card(91, 3, "Blue"));
        cardList.Add(new Card(92, 4, "Blue"));
        cardList.Add(new Card(93, 5, "Blue"));
        cardList.Add(new Card(94, 6, "Blue"));
        cardList.Add(new Card(95, 7, "Blue"));
        cardList.Add(new Card(96, 8, "Blue"));
        cardList.Add(new Card(97, 9, "Blue"));
        cardList.Add(new Card(98, 10, "Blue"));
        cardList.Add(new Card(99, 11, "Blue"));
        cardList.Add(new Card(100, 12, "Blue"));

        // All Wild Cards
        cardList.Add(new Card(101, 13, "Wild"));
        cardList.Add(new Card(102, 13, "Wild"));
        cardList.Add(new Card(103, 13, "Wild"));
        cardList.Add(new Card(104, 13, "Wild"));
        cardList.Add(new Card(105, 14, "Wild"));
        cardList.Add(new Card(106, 14, "Wild"));
        cardList.Add(new Card(107, 14, "Wild"));
        cardList.Add(new Card(108, 14, "Wild"));



    }
}
