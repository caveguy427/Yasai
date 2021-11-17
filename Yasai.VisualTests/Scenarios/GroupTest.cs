﻿using System.Numerics;
using Yasai.Graphics.Groups;
using Yasai.Graphics.Imaging;
using Yasai.Resources;

namespace Yasai.VisualTests.Scenarios
{
    [TestScenario]
    public class GroupTest : Scenario
    {
        public GroupTest(Game g) : base(g)
        {
            //SingleGroup();
            MultipleGroups();
        }

        public void SingleGroup()
        {
            Group g = new Group();
            g.Add(new Sprite ("ino")
            {
                Size = new Vector2(400)
            });
            Add(g);
        }

        public void MultipleGroups()
        {
            Group a = new Group();
            Group b = new Group();
            Group c = new Group();
            Group d = new Group();
            
            d.Add(new Sprite ("ino"));
            c.Add(d);
            b.Add(c);
            a.Add(b);

            Add(a);
        }

        public override void Load(ContentStore store)
        {
            store.LoadResource("ino.png");
            base.Load(store);
        }
    }
}