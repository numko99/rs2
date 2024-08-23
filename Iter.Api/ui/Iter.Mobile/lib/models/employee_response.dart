import 'package:json_annotation/json_annotation.dart';

part 'employee_response.g.dart';

@JsonSerializable()
class EmployeeResponse {
  dynamic id;
  String? firstName;
  String? lastName;
  String? userId;

  EmployeeResponse({required this.id, required this.firstName, required this.lastName, this.userId});

  factory EmployeeResponse.fromJson(Map<String, dynamic> json) =>
      _$EmployeeResponseFromJson(json);

  Map<String, dynamic> toJson() => _$EmployeeResponseToJson(this);
}
