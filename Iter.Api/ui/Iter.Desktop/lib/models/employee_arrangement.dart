import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/arrangement_search_response.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/employee_response.dart';
import 'package:ui/models/user.dart';

part 'employee_arrangement.g.dart';

@JsonSerializable()
class EmployeeArrangment {
  final String? id;
  final EmployeeResponse? employee;
  final ArrangementSearchResponse? arrangement;

  EmployeeArrangment({
    this.id,
    required this.employee,
    required this.arrangement,
  });

  factory EmployeeArrangment.fromJson(Map<String, dynamic> json) => _$EmployeeArrangmentFromJson(json);

  Map<String, dynamic> toJson() => _$EmployeeArrangmentToJson(this);
}