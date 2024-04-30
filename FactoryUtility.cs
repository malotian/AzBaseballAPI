using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AzBaseballAPI {
    public class FactoryUtility {
        public FactoryDB FactoryDB { get; }

        public FactoryUtility() {
            FactoryDB = new FactoryDB();
        }

        public void FixNewsData() {
            string query = "SELECT post_id, meta_value FROM fawp_postmeta WHERE meta_key = 'factory_news_meta'";
            using (SqlCommand cmd = CreateCommand(query)) {
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        string metaValue = reader["meta_value"].ToString();
                        Dictionary<string, string> metaArray = DeserializeMetaValue(metaValue);

                        int postId = Convert.ToInt32(reader["post_id"]);
                        int newsId = Convert.ToInt32(metaArray["news_press_id"]);

                        string newsContentQuery = "SELECT content from factory_news_data WHERE news_press_id = @newsId";
                        using (SqlCommand contentCmd = CreateCommand(newsContentQuery)) {
                            contentCmd.Parameters.AddWithValue("@newsId", newsId);
                            using (SqlDataReader contentReader = contentCmd.ExecuteReader()) {
                                if (contentReader.Read()) {
                                    string newsContent = contentReader["content"].ToString();
                                    // Uncomment the following line if you want to update the post_content
                                    // UpdatePostContent(postId, newsContent);
                                }
                            }
                        }
                    }
                }
            }
        }

        private SqlCommand CreateCommand(string query) {
            return new SqlCommand(query, FactoryDB.Conn);
        }

        private Dictionary<string, object> CreateDictionary(SqlDataReader reader, params string[] fields) {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var field in fields) {
                dictionary[field] = reader[field].ToString();
            }
            return dictionary;
        }

        private Dictionary<string, string> DeserializeMetaValue(string metaValue) {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(metaValue);
        }

        private void UpdatePostContent(int postId, string newsContent) {
            // Implement your post content update logic here
            throw new NotImplementedException();
        }

        public void SetNewsData() {
            string newsQuery = "SELECT * FROM factory_news_data";

            using (SqlCommand newsCmd = new SqlCommand(newsQuery, FactoryDB.Conn)) {
                using (SqlDataReader newsReader = newsCmd.ExecuteReader()) {
                    while (newsReader.Read()) {
                        Dictionary<string, object> newsItem = new Dictionary<string, object> {
                            { "post_title", newsReader["PostTitle"] },
                            { "post_content", newsReader["PostContent"] },
                            { "post_date", newsReader["PostDate"] },
                            { "post_status", "publish" },
                            { "post_author", 1 }
                        };

                        int insertedPostId = InsertPost(newsItem);

                        Dictionary<string, object> newsMeta = new Dictionary<string, object> {
                            { "news_press_id", newsReader["NewsPressId"] },
                            { "source", newsReader["Source"] },
                            { "author", newsReader["Author"] },
                            { "news_press_type_name", newsReader["NewsType"] }
                        };

                        UpdatePostMeta(insertedPostId, "FactoryNewsMeta", newsMeta);
                    }
                }
            }
        }

        public void SetVideoData() {
            string videoQuery = @"SELECT *, CONVERT(varchar(23),added_date,121) as converted_date,
                              COALESCE('-' + video_name_line2, '') as video_name_line_2 
                              FROM tbl_bftv_video_category a 
                              LEFT OUTER JOIN tbl_bftv_category b ON a.fk_bftv_category = b.pk_bftv_category 
                              LEFT OUTER JOIN tbl_bftv_video c ON a.fk_bftv_video = c.pk_bftv_video 
                              WHERE a.fk_bftv_category IN (100,101,102,103) 
                              ORDER BY displayOrder, video_date DESC, added_date DESC";

            using (SqlCommand videoCmd = new SqlCommand(videoQuery, FactoryDB.Conn)) {
                using (SqlDataReader videoReader = videoCmd.ExecuteReader()) {
                    Dictionary<int, int> catMap = new Dictionary<int, int> {
                    { 100, 47 },
                    { 101, 46 },
                    { 102, 29 },
                    { 103, 45 }
                };

                    while (videoReader.Read()) {
                        Dictionary<string, object> video = new Dictionary<string, object> {
                            { "video_name", videoReader["VideoName"] },
                            { "video_name_line1", videoReader["VideoNameLine1"] },
                            { "video_name_line2", videoReader["VideoNameLine2"] },
                            { "video_name_line_2", videoReader["VideoNameLine_2"] },
                            { "video_description", videoReader["VideoDescription"] },
                            { "video_url", videoReader["VideoUrl"] },
                            { "video_length", videoReader["VideoLength"] },
                            { "video_thumbnail_url", videoReader["VideoThumbnailUrl"] },
                            { "converted_date", videoReader["ConvertedDate"] },
                            { "video_id", videoReader["VideoId"] },
                            { "category_id", videoReader["CategoryId"] }
                        };

                        string postTitle = StripTags($"{video["VideoName"]}, {video["VideoNameLine1"]}, {video["VideoNameLine2"]}, {video["VideoNameLine_2"]}");
                        string postContent = StripTags(video["VideoDescription"].ToString());

                        Dictionary<string, object> videoPost = new Dictionary<string, object> {
                            { "post_title", postTitle },
                            { "post_content", postContent },
                            { "post_date", video["ConvertedDate"] },
                            { "post_type", "video" },
                            { "post_status", "publish" },
                            { "post_author", 1 }
                        };

                        int insertedPostId = InsertPost(videoPost);

                        Dictionary<string, object> videoMeta = new Dictionary<string, object> {
                            { "video_id", video["VideoId"] },
                            { "video_thumbnail_url", video["VideoThumbnailUrl"] }
                        };

                        UpdatePostMeta(insertedPostId, "FactoryVideoMeta", videoMeta);
                        UpdatePostMeta(insertedPostId, "VideoUrl", video["VideoUrl"]);
                        UpdatePostMeta(insertedPostId, "VideoLength", video["VideoLength"]);

                        WpSetObjectTerms(insertedPostId, catMap[Convert.ToInt32(video["CategoryId"])], "Channels");
                    }
                }
            }
        }

        private string StripTags(string input) {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public int InsertPost(Dictionary<string, object> post) {
            string postTitle = post["post_title"].ToString();
            string postContent = post["post_content"].ToString();
            string postDate = post["post_date"].ToString();
            string postStatus = post["post_status"].ToString();
            int postAuthor = Convert.ToInt32(post["post_author"]);

            string query = @"INSERT INTO wp_posts (post_title, post_content, post_date, post_status, post_author) 
                            VALUES (@PostTitle, @PostContent, @PostDate, @PostStatus, @PostAuthor);
                            SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(query, FactoryDB.Conn)) {
                cmd.Parameters.AddWithValue("@PostTitle", postTitle);
                cmd.Parameters.AddWithValue("@PostContent", postContent);
                cmd.Parameters.AddWithValue("@PostDate", postDate);
                cmd.Parameters.AddWithValue("@PostStatus", postStatus);
                cmd.Parameters.AddWithValue("@PostAuthor", postAuthor);

                int insertedPostId = Convert.ToInt32(cmd.ExecuteScalar());
                return insertedPostId;
            }
        }

        public void UpdatePostMeta(int postId, string metaKey, object metaValue) {
            string query = @"
                IF EXISTS (SELECT * FROM wp_postmeta WHERE post_id = @PostId AND meta_key = @MetaKey)
                BEGIN
                    UPDATE wp_postmeta SET meta_value = @MetaValue WHERE post_id = @PostId AND meta_key = @MetaKey;
                END
                ELSE
                BEGIN
                    INSERT INTO wp_postmeta (post_id, meta_key, meta_value) VALUES (@PostId, @MetaKey, @MetaValue);
                END";

            using (SqlCommand cmd = new SqlCommand(query, FactoryDB.Conn)) {
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@MetaKey", metaKey);
                cmd.Parameters.AddWithValue("@MetaValue", metaValue.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        public void WpSetObjectTerms(int objectId, int termId, string taxonomy) {
            string query = @"
                IF NOT EXISTS (SELECT * FROM wp_term_relationships WHERE object_id = @ObjectId AND term_taxonomy_id = @TermId)
                BEGIN
                    INSERT INTO wp_term_relationships (object_id, term_taxonomy_id, term_order) 
                    VALUES (@ObjectId, @TermId, 0);
                END";

            using (SqlCommand cmd = new SqlCommand(query, FactoryDB.Conn)) {
                cmd.Parameters.AddWithValue("@ObjectId", objectId);
                cmd.Parameters.AddWithValue("@TermId", termId);

                cmd.ExecuteNonQuery();
            }
        }

        public void SetTestyData() {
            string testyQuery = "SELECT title, testimonial, CONVERT(varchar(23),date,121) as date, author, testimonialID FROM tbl_testimonial order by 1";

            using (SqlCommand testyCmd = new SqlCommand(testyQuery, FactoryDB.Conn)) {
                using (SqlDataReader testyReader = testyCmd.ExecuteReader()) {
                    Dictionary<int, int> catMap = new Dictionary<int, int> {
                { 1, 28 }, { 2, 30 }, { 3, 31 }, { 4, 32 }, { 5, 33 }, { 6, 34 }
            };

                    while (testyReader.Read()) {
                        int testimonialID = Convert.ToInt32(testyReader["testimonialID"]);

                        string testyCatsQuery = $"SELECT * FROM tbl_testimonial_category WHERE fk_testimonialID = {testimonialID} order by 1";

                        using (SqlCommand testyCatsCmd = new SqlCommand(testyCatsQuery, FactoryDB.Conn)) {
                            using (SqlDataReader testyCatsReader = testyCatsCmd.ExecuteReader()) {
                                List<int> catArray = new List<int>();

                                while (testyCatsReader.Read()) {
                                    int catId = Convert.ToInt32(testyCatsReader["fk_testimonialCategoryID"]);
                                    if (catMap.ContainsKey(catId)) {
                                        catArray.Add(catMap[catId]);
                                    }
                                }

                                Dictionary<string, object> testyPost = new Dictionary<string, object> {
                                    { "post_title", testyReader["title"] },
                                    { "post_content", testyReader["testimonial"] },
                                    { "post_date", testyReader["date"] },
                                    { "post_type", "testimonial" },
                                    { "post_status", "publish" },
                                    { "post_author", 1 }
                                };

                                int insertedPostId = InsertPost(testyPost);

                                WpSetObjectTerms(insertedPostId, catArray, "types");

                                UpdatePostMeta(insertedPostId, "FactoryTestimonialId", testyReader["testimonialID"]);
                            }
                        }
                    }
                }
            }
        }

        private void WpSetObjectTerms(int objectId, List<int> termIds, string taxonomy) {
            foreach (int termId in termIds) {
                string query = @"
            IF NOT EXISTS (SELECT * FROM wp_term_relationships WHERE object_id = @ObjectId AND term_taxonomy_id = @TermId)
            BEGIN
                INSERT INTO wp_term_relationships (object_id, term_taxonomy_id, term_order) 
                VALUES (@ObjectId, @TermId, 0);
            END";

                using (SqlCommand cmd = new SqlCommand(query, FactoryDB.Conn)) {
                    cmd.Parameters.AddWithValue("@ObjectId", objectId);
                    cmd.Parameters.AddWithValue("@TermId", termId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Dispose() {
            FactoryDB.Conn.Close();
        }
    }
}
