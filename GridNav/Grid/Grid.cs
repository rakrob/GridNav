using System;
using System.Collections.Generic;

namespace GridNav.Grid
{
    public class Grid
    {
        public readonly float height;
        public readonly float width;
        public readonly float startX;
        public readonly float startY;
        public readonly float endX;
        public readonly float endY;
        public readonly List<Obstruction> objList;
        private Random random;

        public Grid(float gridHeight, float gridWidth, float pctObstructed, int numObstructions, float pathStartX, float pathStartY, float pathEndX, float pathEndY)
        {
            //initialize properties
            height = gridHeight; width = gridWidth; startX = pathStartX; startY = pathStartY; endX = pathEndX; endY = pathEndY;
            objList = new List<Obstruction>();
            random = new Random((int)DateTime.Now.Ticks);

            //generate obstructions
            generateObstructions(gridHeight, gridWidth, pctObstructed, numObstructions, startX, startY, endX, endY);
        }

        private void generateObstructions(float gridHeight, float gridWidth, float pctObstructed, int numObstructions, float startX, float startY, float endX, float endY)
        {
            //establish our parameters, total area, area we want blocked, and area blocked thus far by the loop
            float gridArea = gridHeight * gridWidth;
            float obstructedArea = gridArea * pctObstructed / 100;

            //reserve memory for variables that we will assign during loop, but don't need to be declared every iteration
            float requiredArea;

            //create the requested number of obstructions, each having equal area
            //this can eventually change based on what the requirements are for grid navigation are, it's just a good start for now
            for (int rollingNumObstructions = 0; rollingNumObstructions < numObstructions; rollingNumObstructions++)
            {
                requiredArea = obstructedArea / numObstructions;

                //let's say for simplicity's sake we don't want anything larger than a 1:5 aspect ratio for our obstructions
                //then, our random width that we generate will deviate from 1/5-5x of what our normalized value for x is
                float normalizedValue = 5;
                float squareSide = (float)Math.Pow((double)requiredArea, .5);
                float obsWidth;
                float obsHeight;
                float obsOriginX;
                float obsOriginY;
                do
                {

                    if (getRandomBool())
                    {
                        obsWidth = getRandomfloat(1, normalizedValue) * squareSide;
                    }
                    else
                    {
                        obsWidth = getRandomfloat(1 / normalizedValue, 1) * squareSide;
                    }
                    
                    obsHeight = requiredArea / obsWidth;

                    //now that we have our randomized width/height, we need to place it in the grid where it won't intersect any other rectangle
                    obsOriginX = getRandomfloat(20, gridWidth - obsWidth - 20);
                    obsOriginY = getRandomfloat(20, gridHeight - obsHeight - 20);

                } while (checkCollisions(obsOriginX, obsOriginY, obsWidth, obsHeight, startX, startY, endX, endY));

                objList.Add(new Obstruction(obsOriginX, obsOriginY, obsHeight, obsWidth));
            }
        }

        private bool checkCollisions(float X, float Y, float W, float H, float startX, float startY, float endX, float endY)
        {
            Obstruction checkObs = new Obstruction(X, Y, H, W);

            //obviously we don't want our start and end points intersecting with any obstructions
            if (checkObs.isInside(startX, startY)) return true;
            if (checkObs.isInside(endX, endY)) return true;

            //check if we are also intersecting anything that is already established
            foreach (Obstruction obj in objList)
            {
                if (obj.isColliding(checkObs)) return true;
            }
            return false;
        }

        private float getRandomfloat(float min, float max)
        {
            return (1 - (float)random.NextDouble()) * (max - min) + min;
        }

        private bool getRandomBool()
        {
            if (random.Next(0, 2) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
