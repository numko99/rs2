import 'dart:convert';

import 'package:ui/models/arrangement_earnings.dart';
import 'package:ui/models/user.dart';
import 'package:ui/models/user_payment.dart';
import 'base_provider.dart';

class UserProvider extends BaseProvider<User, User> {
  UserProvider() : super("User");

  @override
  User fromJson(data) {
    return User.fromJson(data);
  }

  @override
  User fromJsonSearch(data) {
    return User.fromJson(data);
  }

  Future<User?> newPassword(var id) async {
    var url = Uri.parse("$baseUrl$endpoint/new-password/$id");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.put(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data) as User;
    } else {
      return null;
    }
  }
}