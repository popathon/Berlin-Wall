using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[XmlRoot("SegmentData")]
public class WallSegmentData
{

	[XmlArray("Segments")]
	[XmlArrayItem("Segment")]
	public List<WallSegmentObject> Segments = new List<WallSegmentObject>();

	public WallSegmentData()
	{
		if (File.Exists("./walldata.xml"))
		{
			ReadData();
		}
	}

	private void ReadData()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(WallSegmentData));
		FileStream stream = new FileStream("./walldata.xml", FileMode.Open);
		WallSegmentData container = serializer.Deserialize(stream) as WallSegmentData;
		stream.Close();
		this.Segments = container.Segments;
	}

	public void SaveData()
	{
		UpdateData();

		XmlSerializer serializer = new XmlSerializer(typeof(WallSegmentData));
		FileStream stream = new FileStream("./walldata.xml", FileMode.OpenOrCreate);
		serializer.Serialize(stream, this);
		stream.Close();
	}

	private void UpdateData()
	{
		Segments.Clear();

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		for (int i = 0; i < walls.Length; i++)
		{
			WallSegmentObject segment = new WallSegmentObject();

			Segments.Add(segment);
		}
	}
}

public class WallSegmentObject
{
	[XmlArray("Texts")]
	[XmlArrayItem("Text")]
	public List<WallSegmentTextObject> Texts = new List<WallSegmentTextObject>();

	[XmlArray("Images")]
	[XmlArrayItem("Image")]
	public List<WallSegmentImageObject> Images = new List<WallSegmentImageObject>();

	public WallSegmentObject()
	{
	}

	public WallSegmentObject(GameObject parent)
	{
		Texts.Clear();
		Text[] textChildren = parent.GetComponentsInChildren<Text>();
		foreach (Text text in textChildren)
		{
			WallSegmentTextObject textObject = new WallSegmentTextObject(text.text,
				text.transform.localPosition.z, text.transform.localPosition.y, text.transform.localEulerAngles.z);
			Texts.Add(textObject);
		}
		/*
		Images.Clear();
		MeshRenderer[] imageChildren = parent.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in imageChildren)
		{
			WallSegmentImageObject textObject = new WallSegmentImageObject(text.text,
				text.transform.localPosition.z, text.transform.localPosition.y, text.transform.localEulerAngles.z);
			Texts.Add(textObject);
		}
		 */
	}
}

public class WallSegmentTextObject
{
	public WallSegmentTextObject()
	{
	}

	public WallSegmentTextObject(string text, float x, float y, float rotation)
	{
		this.text = text;
		this.x = x;
		this.y = y;
		this.rotation = rotation;
	}

	public string text;

	public float x, y, rotation;
}

public class WallSegmentImageObject
{
	public string image;
	public Texture2D texture;

	public float x, y, rotation;

	public WallSegmentImageObject()
	{
	}

	public WallSegmentImageObject(string image, float x, float y, float rotation)
	{
		this.image = image;
		this.x = x;
		this.y = y;
		this.rotation = rotation;
	}

	public void LoadTexture()
	{
		WWW www = new WWW(this.image);
		this.texture = new Texture2D(1, 1);
		www.LoadImageIntoTexture(this.texture);
	}
}