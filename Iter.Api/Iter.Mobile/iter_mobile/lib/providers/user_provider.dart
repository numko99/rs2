import 'dart:convert';
import 'package:iter_mobile/models/user.dart';
import 'package:iter_mobile/models/user_statistic_data.dart';
import 'base_provider.dart';

class UserProvider extends BaseProvider<User, User> {
  UserProvider() : super("User");

  Future<User?> newPassword(var id) async {
    var url = Uri.parse("$baseUrl$endpoint/new-password/$id");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.put(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      return null;
    }
  }

    Future updatePassword(String currentPassword, String newPassword) async {
    var url = Uri.parse("$baseUrl$endpoint/update-password");

    Map<String, String>? headers = await createHeaders();

    Map<String, String> body2 = {
      'currentPassword': currentPassword,
      'newPassword': newPassword,
    };

    var response = await http!.post(url, headers: headers,  body: jsonEncode(body2));

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      return null;
    }
  }

    Future<User> getCurrentUser() async {
    var url = Uri.parse("$baseUrl$endpoint/currentUser");

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(url, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }

  Future<UserStatisticData> getCurrentUserStatisticData() async {
    var url = Uri.parse("$baseUrl$endpoint/current-user-statistic-data");

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(url, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return UserStatisticData.fromJson(data);
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }

    Future<User?> updateCurrentUser([dynamic request]) async {
    var url = "$baseUrl$endpoint/currentUser";
    var uri = Uri.parse(url);

    Map<String, String>? headers = await createHeaders();

    var response =
        await http!.post(uri, headers: headers, body: jsonEncode(request));

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      return null;
    }
  }

  @override
  User fromJson(data) {
    return User.fromJson(data);
  }
}