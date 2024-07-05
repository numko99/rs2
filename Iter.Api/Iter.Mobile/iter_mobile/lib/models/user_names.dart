import 'package:json_annotation/json_annotation.dart';

part 'user_names.g.dart';

@JsonSerializable()

class UserNamesResponse {
  final String id;
  final String firstName;
  final String lastName;
  final String? agencyName;

  UserNamesResponse({
    required this.id,
    required this.firstName,
    required this.lastName,
    this.agencyName,
  });

  
  factory UserNamesResponse.fromJson(Map<String, dynamic> json) =>
      _$UserNamesResponseFromJson(json);

  Map<String, dynamic> toJson() => _$UserNamesResponseToJson(this);
}
