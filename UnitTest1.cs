using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using SoftraySolutions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftraySolutions
{
    [TestFixture]
    public class GetSingleCollectionEndPoint
    {
        List<PostResponse> posts = new List<PostResponse>();
        public string SetYourAPIKey = "PMAK-620a6310166fbb414a6cc9f6-8d67ac21f64e22d89001acae70d7a94b12";
        public void AddThreeCollections()
        {
            posts.Clear();

            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat=DataFormat.Json;

            Post post1 = new Post() { collection = new Collection() { info = new Info() { name = "Softray Solutions 1", schema = "Schema 1" }, item = new List<Item>() } };

            request.AddBody(post1);

            var task1 = client.ExecuteAsync<List<Post>>(request);

            // 

            var request2 = new RestRequest("collections", Method.Post);

            request2.AddHeader("X-Api-Key", SetYourAPIKey);

            request2.RequestFormat = DataFormat.Json;

            Post post2 = new Post() { collection = new Collection() { info = new Info() { name = "Softray Solutions 2", schema = "Schema 2" }, item = new List<Item>() } };

            request2.AddBody(post2);

            var task2 = client.ExecuteAsync<List<Post>>(request2);
            //

            var request3= new RestRequest("collections", Method.Post);

            request3.AddHeader("X-Api-Key", SetYourAPIKey);

            request3.RequestFormat = DataFormat.Json;

            Post post3 = new Post() { collection = new Collection() { info = new Info() { name = "Softray Solutions 3", schema = "Schema 3" }, item = new List<Item>() } };

            request3.AddBody(post3);

            var task3= client.ExecuteAsync<List<Post>>(request3);

            //

            var tasks = new List<Task> { task1, task2, task3 };

            Task.WhenAll(tasks);

            JObject obs = JObject.Parse(task1.Result.Content);

            var name1 = obs["collection"]["name"];
            var id1 = obs["collection"]["id"];
            var uid1 = obs["collection"]["uid"];

            PostResponse postResponse1 = new PostResponse() { collection = new Collection() { info = new Info() { name = name1.ToString(), schema= post1.collection.info.schema }, item= new List<Item>(), id = id1.ToString(), uid = uid1.ToString() } };

            obs = JObject.Parse(task2.Result.Content);

            var name2 = obs["collection"]["name"];
            var id2 = obs["collection"]["id"];
            var uid2 = obs["collection"]["uid"];

            PostResponse postResponse2 = new PostResponse() { collection = new Collection() { info = new Info() { name = name2.ToString(), schema = post1.collection.info.schema }, item = new List<Item>(), id = id2.ToString(), uid = uid2.ToString() } };

            obs = JObject.Parse(task3.Result.Content);

            var name3 = obs["collection"]["name"];
            var id3 = obs["collection"]["id"];
            var uid3 = obs["collection"]["uid"];

            PostResponse postResponse3 = new PostResponse() { collection = new Collection() { info = new Info() { name = name3.ToString() , schema = post1.collection.info.schema }, item = new List<Item>(), id = id3.ToString(), uid = uid3.ToString() } };

            posts.Add(postResponse1);
            posts.Add(postResponse2);
            posts.Add(postResponse3);
        }
        [Test]
        public void GetWithExistingUid()
        {
            AddThreeCollections();

            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections/"+posts[0].collection.uid, Method.Get);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            var response = client.ExecuteAsync(request).Result;

            JObject obs = JObject.Parse(response.Content);

            Assert.IsTrue((int)response.StatusCode == 200 || (int)response.StatusCode == 201);
            Assert.IsTrue(response.ErrorMessage == null);
            Assert.IsTrue(obs["collection"]["info"]["_postman_id"].ToString() == posts[0].collection.id);
            Assert.IsTrue(obs["collection"]["info"]["name"].ToString() == posts[0].collection.info.name);
            Assert.IsTrue(obs["collection"]["info"]["schema"].ToString() == posts[0].collection.info.schema);
        }
        [Test]
        public void GetWithNonExistingUid()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections/nonExistingUid", Method.Get);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            var response = client.ExecuteAsync(request).Result;

            JObject obs = JObject.Parse(response.Content);

            Assert.IsTrue((int)response.StatusCode == 404);
            Assert.IsTrue(response.ErrorException!=null);
            Assert.IsTrue(obs["error"]["message"].ToString() == "We could not find the collection you are looking for");
            Assert.IsTrue(obs["error"]["name"].ToString() == "instanceNotFoundError");
        }
        [Test]
        public void GetWithInvalidUid()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections/đđđđđđ", Method.Get);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            var response = client.ExecuteAsync(request).Result;

            JObject obs = JObject.Parse(response.Content);

            Assert.IsTrue((int)response.StatusCode == 400);
            Assert.IsTrue(response.ErrorException != null);

        }
        //Can not assert response in case of an already existing collection, since the parameters "name" and "schema" must not be unique.
    }
    [TestFixture]
    public class PostCollectionEndPoint
    {
        public string SetYourAPIKey = "PMAK-620a6310166fbb414a6cc9f6-8d67ac21f64e22d89001acae70d7a94b12";
        [Test]
        public void PostCollection()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat = DataFormat.Json;

            Post postCollection = new Post() { collection = new Collection() { info = new Info() { name = "Softray Solutions Post Test", schema = "Schema Post Test" }, item = new List<Item>() } };

            request.AddBody(postCollection);

            var response = client.ExecuteAsync(request).Result;

            JObject obs = JObject.Parse(response.Content);

            Assert.IsTrue((int)response.StatusCode == 200 || (int)response.StatusCode == 201);
            Assert.IsTrue(obs["collection"]["id"].ToString() != null);
            Assert.IsTrue(obs["collection"]["name"].ToString() == postCollection.collection.info.name);
            Assert.IsTrue(obs["collection"]["uid"].ToString() != null);
        }
        [Test]
        public void PostCollectionWithoutName()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat = DataFormat.Json;

            Post postCollection = new Post() { collection = new Collection() { info = new Info() { schema = "Schema Post Test" }, item = new List<Item>() } };

            request.AddBody(postCollection);

            var response = client.ExecuteAsync(request).Result;

            Assert.IsTrue((int)response.StatusCode == 400);
            Assert.IsTrue(response.ErrorException != null);
        }
        [Test]
        public void PostCollectionWithoutSchema()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat = DataFormat.Json;

            Post postCollection = new Post() { collection = new Collection() { info = new Info() { name= "Softray Solutions Post Test" }, item = new List<Item>() } };

            request.AddBody(postCollection);

            var response = client.ExecuteAsync(request).Result;

            Assert.IsTrue((int)response.StatusCode == 400);
            Assert.IsTrue(response.ErrorException != null);
        }
        [Test]
        public void PostCollectionWithoutItem()
        {
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat = DataFormat.Json;

            Post postCollection = new Post() { collection = new Collection() { info = new Info() {name= "Softray Solutions Post Test", schema = "Schema Post Test" } } };

            request.AddBody(postCollection);

            var response = client.ExecuteAsync(request).Result;

            Assert.IsTrue((int)response.StatusCode == 400);
            Assert.IsTrue(response.ErrorException != null);
        }
        [Test]
        public void PostCollectionWithRequiredFieldsEmpty()
        {
            //Postman documentation shows fields "name","schema" and the array Item are all required
            var client = new RestClient("https://api.getpostman.com/");

            var request = new RestRequest("collections", Method.Post);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            request.RequestFormat = DataFormat.Json;

            Post postCollection = new Post() { collection = new Collection() { info = new Info() {name="",schema="" }, item = new List<Item>() } };

            request.AddBody(postCollection);

            var response = client.ExecuteAsync(request).Result;

            Assert.IsTrue((int)response.StatusCode == 400);
            Assert.IsTrue(response.ErrorException != null);
        }
    }
    [TestFixture]
    public class GetCollectionEndPoint
    {
        public string SetYourAPIKey = "PMAK-620a6310166fbb414a6cc9f6-8d67ac21f64e22d89001acae70d7a94b12";
        [Test]
        public void GetCollection()
        {
            RestClient client = new RestClient("https://api.getpostman.com/");

            RestRequest request = new RestRequest("collections", Method.Get);

            request.AddHeader("X-Api-Key", SetYourAPIKey);

            RestResponse response = client.ExecuteAsync(request).Result;

            JObject obs = JObject.Parse(response.Content);

            Assert.IsTrue((int)response.StatusCode == 200);
            Assert.IsTrue(response.ErrorException == null);
            Assert.IsTrue(obs["collections"][0]["id"].ToString()!=null);
            Assert.IsTrue(obs["collections"][0]["name"].ToString() != null);
            Assert.IsTrue(obs["collections"][0]["owner"].ToString() != null);
            Assert.IsTrue(obs["collections"][0]["createdAt"].ToString() != null);
            Assert.IsTrue(obs["collections"][0]["updatedAt"].ToString() != null);
            Assert.IsTrue(obs["collections"][0]["uid"].ToString() != null);
            Assert.IsTrue(obs["collections"][0]["isPublic"].ToString() != null);
        }
    }
}