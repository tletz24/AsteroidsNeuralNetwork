﻿using System.Collections.Generic;
using System.Linq;
using Baldwin;
using UnityEngine;

namespace Asteroids
{
	public class AsteroidSpawner : Singleton<AsteroidSpawner>
	{
		public GameObject asteroidPrefab;
		public int numOfRoidsToSpawn = 10;
		public int minLargeRoids = 3;
		public float largeRoidScale = 3.0f;
		public float mediumRoidScale = 2.0f;
		public float normalRoidScale = 1.0f;
		public float smallRoidScale = 0.5f;
		public float tinyRoidScale = 0.25f;

		private void Start()
		{
			Restart();
		}

		private void Update()
		{
			List<Asteroid> allRoids = FindObjectsOfType<Asteroid>().ToList();
			int largeRoidCount = 0;
			foreach(var iRoid in allRoids)
			{
				if(iRoid.size == AsteroidSize.LARGE)
				{
					largeRoidCount++;
				}
			}

			if(largeRoidCount < minLargeRoids)
			{
				for(int i = 0; i < minLargeRoids - largeRoidCount; i++)
				{
					Spawn();
				}
			}
		}

		public void Restart()
		{
			foreach(var iRoid in FindObjectsOfType<Asteroid>())
			{
				Destroy(iRoid.gameObject);
			}

			for(int i = 0; i < numOfRoidsToSpawn; i++)
			{
				Spawn();
			}
		}

		public void Spawn()
		{
			Vector3 startPos = Camera.main.ViewportToWorldPoint(Extensions.OutsideOfUnitBox());
			Spawn(startPos, AsteroidSize.LARGE);
		}

		public void Spawn(Vector2 pos, AsteroidSize size)
		{
			Asteroid asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity).GetComponent<Asteroid>();
			asteroid.size = size;
			asteroid.transform.localScale = Vector3.one * GetScaleFromSize(size);
			asteroid.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * (100.0f / GetScaleFromSize(size)));
			//asteroid.GetComponent<Rigidbody2D>().AddForce((FindObjectOfType<Ship>().transform.position - asteroid.transform.position).normalized * (100.0f / GetScaleFromSize(size)));
			asteroid.GetComponent<Rigidbody2D>().AddTorque(3);
		}

		public float GetScaleFromSize(AsteroidSize size)
		{
			switch(size)
			{
				case AsteroidSize.LARGE:
					return largeRoidScale;
				case AsteroidSize.MEDIUM:
					return mediumRoidScale;
				case AsteroidSize.NORMAL:
					return normalRoidScale;
				case AsteroidSize.SMALL:
					return smallRoidScale;
				case AsteroidSize.TINY:
					return tinyRoidScale;
			}

			return normalRoidScale;
		}
	}
}
