import 'package:flutter/material.dart';

class ExpandableText extends StatefulWidget {
  final String text;
  final int trimLength; // Broj karaktera do kojeg će se tekst skratiti

  const ExpandableText({Key? key, required this.text, this.trimLength = 100})
      : super(key: key);

  @override
  _ExpandableTextState createState() => _ExpandableTextState();
}

class _ExpandableTextState extends State<ExpandableText> {
  late String firstPart;
  late String secondPart;
  bool isExpanded = false;

  @override
  void initState() {
    super.initState();
    if (widget.text.length > widget.trimLength) {
      firstPart = widget.text.substring(0, widget.trimLength);
      secondPart = widget.text.substring(widget.trimLength, widget.text.length);
    } else {
      firstPart = widget.text;
      secondPart = '';
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          isExpanded
              ? widget.text
              : (secondPart.isEmpty ? firstPart : '$firstPart...'),
          style: TextStyle(fontSize: 16),
        ),
        InkWell(
          child: secondPart.isEmpty
              ? SizedBox()
              : Text(
                  isExpanded ? 'Sakrij' : 'Prikaži ostalo',
                  style: TextStyle(
                      color: Colors.amber, fontWeight: FontWeight.bold),
                ),
          onTap: () {
            setState(() {
              isExpanded = !isExpanded;
            });
          },
        )
      ],
    );
  }
}
