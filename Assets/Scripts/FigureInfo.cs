using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureInfo
{
	public List<List<BlockInfo>> m_rotationInfos;
	public int m_numLeftRotCheck;
	public int m_numRightRotCheck;
	public Color32 m_blocksColor;
    public int m_indexRotationAxisBlock;
    public int m_blocksCount;

	public FigureInfo(int figureID) 
	{
		m_rotationInfos = new List<List<BlockInfo>> ();
		//I-figure - 1
		#region
		if (figureID == 1) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 2;
			m_blocksColor = new Color32 (255, 113, 113, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 0));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (1, 3));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (0, 1));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (3, 1));
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
		}
		#endregion

		//L left - 2
		#region
		if (figureID == 2) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (113, 255, 113, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (1, 0));
			blockInfos1.Add (new BlockInfo (2, 0));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (0, 1));
			blockInfos2.Add (new BlockInfo (0, 0));
			//State 2
			List<BlockInfo> blockInfos3 = new List<BlockInfo>();
			blockInfos3.Add (new BlockInfo (1, 0));
			blockInfos3.Add (new BlockInfo (1, 1));
			blockInfos3.Add (new BlockInfo (1, 2));
			blockInfos3.Add (new BlockInfo (0, 2));
			//State 2
			List<BlockInfo> blockInfos4 = new List<BlockInfo>();
			blockInfos4.Add (new BlockInfo (0, 1));
			blockInfos4.Add (new BlockInfo (1, 1));
			blockInfos4.Add (new BlockInfo (2, 1));
			blockInfos4.Add (new BlockInfo (2, 2));
			//Add
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
			m_rotationInfos.Add (blockInfos3);
			m_rotationInfos.Add (blockInfos4);
		}
		#endregion

		//L Right - 3
		#region
		if (figureID == 3) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (113, 255, 113, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (1, 0));
			blockInfos1.Add (new BlockInfo (0, 0));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (0, 1));
			blockInfos2.Add (new BlockInfo (0, 2));
			//State 2
			List<BlockInfo> blockInfos3 = new List<BlockInfo>();
			blockInfos3.Add (new BlockInfo (1, 0));
			blockInfos3.Add (new BlockInfo (1, 1));
			blockInfos3.Add (new BlockInfo (1, 2));
			blockInfos3.Add (new BlockInfo (2, 2));
			//State 2
			List<BlockInfo> blockInfos4 = new List<BlockInfo>();
			blockInfos4.Add (new BlockInfo (0, 1));
			blockInfos4.Add (new BlockInfo (1, 1));
			blockInfos4.Add (new BlockInfo (2, 1));
			blockInfos4.Add (new BlockInfo (2, 0));
			//Add
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
			m_rotationInfos.Add (blockInfos3);
			m_rotationInfos.Add (blockInfos4);
		}
		#endregion

		//Square - 4
		#region
		if (figureID == 4) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (113, 113, 255, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 0));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (2, 1));
			blockInfos1.Add (new BlockInfo (2, 0));
			//Add
			m_rotationInfos.Add (blockInfos1);
		}
		#endregion

		//T - 5
		#region
		if (figureID == 5) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (255, 255, 113, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (0, 1));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (2, 1));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (1, 2));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (1, 0));
			//State 2
			List<BlockInfo> blockInfos3 = new List<BlockInfo>();
			blockInfos3.Add (new BlockInfo (2, 1));
			blockInfos3.Add (new BlockInfo (1, 1));
			blockInfos3.Add (new BlockInfo (1, 0));
			blockInfos3.Add (new BlockInfo (0, 1));
			//State 2
			List<BlockInfo> blockInfos4 = new List<BlockInfo>();
			blockInfos4.Add (new BlockInfo (1, 0));
			blockInfos4.Add (new BlockInfo (1, 1));
			blockInfos4.Add (new BlockInfo (0, 1));
			blockInfos4.Add (new BlockInfo (1, 2));
			//Add
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
			m_rotationInfos.Add (blockInfos3);
			m_rotationInfos.Add (blockInfos4);
		}
		#endregion

		//S Left - 6
		#region
		if (figureID == 6) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (113, 255, 255, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (0, 1));
			blockInfos1.Add (new BlockInfo (0, 0));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (1, 2));
			blockInfos2.Add (new BlockInfo (0, 2));
			//State 2
			List<BlockInfo> blockInfos3 = new List<BlockInfo>();
			blockInfos3.Add (new BlockInfo (1, 0));
			blockInfos3.Add (new BlockInfo (1, 1));
			blockInfos3.Add (new BlockInfo (2, 1));
			blockInfos3.Add (new BlockInfo (2, 2));
			//State 2
			List<BlockInfo> blockInfos4 = new List<BlockInfo>();
			blockInfos4.Add (new BlockInfo (0, 1));
			blockInfos4.Add (new BlockInfo (1, 1));
			blockInfos4.Add (new BlockInfo (1, 0));
			blockInfos4.Add (new BlockInfo (2, 0));
			//Add
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
			m_rotationInfos.Add (blockInfos3);
			m_rotationInfos.Add (blockInfos4);
		}
		#endregion

		//S Right - 7
		#region
		if (figureID == 7) 
		{
            m_blocksCount = 4;
            m_indexRotationAxisBlock = 1;
            m_numLeftRotCheck = 1;
			m_numRightRotCheck = 1;
			m_blocksColor = new Color32 (113, 255, 255, 255);

			//State 1
			List<BlockInfo> blockInfos1 = new List<BlockInfo>();
			blockInfos1.Add (new BlockInfo (1, 2));
			blockInfos1.Add (new BlockInfo (1, 1));
			blockInfos1.Add (new BlockInfo (2, 1));
			blockInfos1.Add (new BlockInfo (2, 0));
			//State 2
			List<BlockInfo> blockInfos2 = new List<BlockInfo>();
			blockInfos2.Add (new BlockInfo (2, 1));
			blockInfos2.Add (new BlockInfo (1, 1));
			blockInfos2.Add (new BlockInfo (1, 0));
			blockInfos2.Add (new BlockInfo (0, 0));
			//State 2
			List<BlockInfo> blockInfos3 = new List<BlockInfo>();
			blockInfos3.Add (new BlockInfo (1, 0));
			blockInfos3.Add (new BlockInfo (1, 1));
			blockInfos3.Add (new BlockInfo (0, 1));
			blockInfos3.Add (new BlockInfo (0, 2));
			//State 2
			List<BlockInfo> blockInfos4 = new List<BlockInfo>();
			blockInfos4.Add (new BlockInfo (0, 1));
			blockInfos4.Add (new BlockInfo (1, 1));
			blockInfos4.Add (new BlockInfo (1, 2));
			blockInfos4.Add (new BlockInfo (2, 2));
			//Add
			m_rotationInfos.Add (blockInfos1);
			m_rotationInfos.Add (blockInfos2);
			m_rotationInfos.Add (blockInfos3);
			m_rotationInfos.Add (blockInfos4);
		}
        #endregion

        //Point - 8
        #region
        if (figureID == 8)
        {
            m_blocksCount = 1;
            m_indexRotationAxisBlock = 0;
            m_numLeftRotCheck = 0;
            m_numRightRotCheck = 0;
            m_blocksColor = new Color32(255, 113, 255, 255);

            //State 1
            List<BlockInfo> blockInfos1 = new List<BlockInfo>();
            blockInfos1.Add(new BlockInfo(1, 1));
            //Add
            m_rotationInfos.Add(blockInfos1);
        }
        #endregion

        //SuperPoint - 9
        #region
        if (figureID == 9)
        {
            m_blocksCount = 1;
            m_indexRotationAxisBlock = 0;
            m_numLeftRotCheck = 0;
            m_numRightRotCheck = 0;
            m_blocksColor = new Color32(255, 255, 255, 255);

            //State 1
            List<BlockInfo> blockInfos1 = new List<BlockInfo>();
            blockInfos1.Add(new BlockInfo(1, 1));
            //Add
            m_rotationInfos.Add(blockInfos1);
        }
        #endregion
    }
}	

public struct BlockInfo 
{
	public int m_localX;
	public int m_localY;

	public BlockInfo(int localX, int localY) 
	{
		m_localX = localX;
		m_localY = localY;
	}
}
