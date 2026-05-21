using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    public static class GameExecutable
    {
        /// <summary>
        /// Rpg example is a simple turn based battle system where the player and an enemy take turns attacking each other until one of them runs out of health.
        /// You cant win, death is inevitable. This is the idea behind Mortis Terminalis, where this VCPU is made for.
        /// </summary>
        public static byte[] RpgExample 
        {
            get
            {
                var program = new List<byte>();

                //Setup default stats
                program.AddRange(new byte[] { (byte)OpCodes.LOAD, (byte)Register.R0, 10 }); //Player Health
                program.AddRange(new byte[] { (byte)OpCodes.RND,  (byte)Register.R1, 3, 8 });  //Enemy Health (Random 3-8)
                program.AddRange(new byte[] { (byte)OpCodes.LOAD, (byte)Register.R2, 3 });  //Player Attack Power
                program.AddRange(new byte[] { (byte)OpCodes.RND,  (byte)Register.R3, 1, 3 });  //Enemy Attack Power (Random 1-3)

                //Loop Start
                int loopStart = program.Count;
                
                //Status Output
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Player HP: \0"));
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R0 });
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes(" | Enemy HP: \0"));
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R1 });
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("\n\0"));

                //Prompt the user to press any number to attack
                //(Note, the input value doesnt do anything its purely for checking input as i havent implemented button inputs yet)
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Enter any number to attack!\n\0"));

                //Pause for input
                program.AddRange(new byte[] { (byte)OpCodes.IPT, 0, (byte)Register.R4 }); 

                //Player attacks enemy
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("\nYou hit for \0"));
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R2 });
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes(" DMG!\n\0"));
                program.AddRange(new byte[] { (byte)OpCodes.SUB, (byte)Register.R1, (byte)Register.R2 });
                program.AddRange(new byte[] { (byte)OpCodes.CMP, (byte)Register.R1, (byte)Register.R5 }); //Checks if the enemy's health is 0 or less, R5 is 0

                program.Add((byte)OpCodes.JE);
                int victoryJumpIndex = program.Count;
                program.Add(0); // Placeholder

                program.Add((byte)OpCodes.JL);
                int victoryJumpIndex2 = program.Count;
                program.Add(0); // Placeholder
                
                // Enemy attacks player
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Enemy hits for \0"));
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R3 });
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes(" DMG!\n\n\0"));
                program.AddRange(new byte[] { (byte)OpCodes.SUB, (byte)Register.R0, (byte)Register.R3 });
                program.AddRange(new byte[] { (byte)OpCodes.CMP, (byte)Register.R0, (byte)Register.R5 }); // Compare with R5 which is 0
                
                program.Add((byte)OpCodes.JE);
                int defeatJumpIndex = program.Count;
                program.Add(0); // Placeholder

                program.Add((byte)OpCodes.JL);
                int defeatJumpIndex2 = program.Count;
                program.Add(0); // Placeholder
                
                //Loop back
                program.AddRange(new byte[] { (byte)OpCodes.JMP, (byte)loopStart });

                //Enemy Defeated / Spawn New Enemy
                program[victoryJumpIndex] = (byte)program.Count;
                program[victoryJumpIndex2] = (byte)program.Count;
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Enemy defeated! A new enemy appears!\n\0"));
                program.AddRange(new byte[] { (byte)OpCodes.RND, (byte)Register.R1, 3, 8 }); //Random Enemy Health
                program.AddRange(new byte[] { (byte)OpCodes.RND, (byte)Register.R3, 1, 3 }); //Random Enemy Attack Power
                program.AddRange(new byte[] { (byte)OpCodes.JMP, (byte)loopStart });

                //Defeat Message
                program[defeatJumpIndex] = (byte)program.Count;
                program[defeatJumpIndex2] = (byte)program.Count;
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("You lose!\n\0"));
                program.Add((byte)OpCodes.END);

                return program.ToArray();
            }
        }

        /// <summary>
        /// Small guessing game where the user has to guess a random number between 1 and 100. 
        /// The program will give feedback if the guess is too high or too low until the user guesses the correct number.
        /// </summary>
        public static byte[] GuessingGame 
        {
            get
            {
                var program = new List<byte>();

                //Generate random number 1 to 100, store in R0
                program.AddRange(new byte[] { (byte)OpCodes.RND, (byte)Register.R0, 1, 100 }); 

                //Loop Start
                int loopStart = program.Count;

                //Prompt user
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Guess a number (1-100): \0"));

                //Pause for input -> stored in R1
                program.AddRange(new byte[] { (byte)OpCodes.IPT, 0, (byte)Register.R1 }); 

                //Compare R1 (guess) with R0 (target)
                program.AddRange(new byte[] { (byte)OpCodes.CMP, (byte)Register.R1, (byte)Register.R0 });
                
                //If equal, jump to victory
                program.Add((byte)OpCodes.JE);
                int victoryJumpIndex = program.Count;
                program.Add(0); //Placeholder
                
                //If less, jump to higher prompt
                program.Add((byte)OpCodes.JL);
                int higherJumpIndex = program.Count;
                program.Add(0); //Placeholder

                //Otherwise, it implies greater, so print "Lower!"
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Lower!\n\0"));
                program.AddRange(new byte[] { (byte)OpCodes.JMP, (byte)loopStart });

                //Target for "Higher!"
                program[higherJumpIndex] = (byte)program.Count;
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("Higher!\n\0"));
                program.AddRange(new byte[] { (byte)OpCodes.JMP, (byte)loopStart });

                //Target for Victory
                program[victoryJumpIndex] = (byte)program.Count;
                program.AddRange(new byte[] { (byte)OpCodes.PRT, (byte)OutputType.String, 2 });
                program.AddRange(Encoding.ASCII.GetBytes("You guessed it!\n\0"));
                program.Add((byte)OpCodes.END);

                return program.ToArray();
            }
        }
    }
}
